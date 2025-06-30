using Checkpoint.IdentityServer.Consumers;
using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Filters;
using Checkpoint.IdentityServer.Interfaces;
using Checkpoint.IdentityServer.Policies;
using Checkpoint.IdentityServer.Services;
using Checkpoint.IdentityServer.TokenServices;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Redis;
using Shared;
using Shared.Events;
using Shared.Hash;

namespace Checkpoint.IdentityServer.DependencyInjections
{
    public static class GeneralDIExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration.GetSection("TokenConf").Get<TokenConf>()!.Issuer,
                    ValidAudience = configuration.GetSection("TokenConf").Get<TokenConf>()!.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Hashing.Hash(configuration.GetSection("TokenConf").Get<TokenConf>()!.ClientSecret))
                };
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            services.Configure<TokenConf>(configuration.GetSection("TokenConf"));
            services.AddScoped<RegisterOutboxTransaction>();
            services.AddScoped<CorporateTransaction>();
            services.AddScoped<CompanyTransaction>();
            services.AddScoped<IIdentityDbContext, IdentityDbContext>();
            services.AddScoped<TokenService>();
            services.AddScoped<ClientTransaction>();
            services.AddScoped<UserService>();
            services.AddSingleton<TokenDto>();
            services.AddSingleton<IAuthorizationHandler, AddRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AddCorporateToTeamHandler>();
            services.AddScoped<TeamService>();
            services.AddSingleton<IRedisClientAsync>(y => new RedisClient("localhost", 6379));
            services.AddSingleton<FillTokenInformationServiceFilter>();


            services.AddDbContext<IdentityDbContext>(y => y.UseNpgsql(configuration.GetConnectionString("checkpoint")));
            services.AddMassTransit<IBus>(configure =>
            {
                configure.AddConsumer<AnalysisNotAvgEventConsumer>();
                configure.AddConsumer<GetAllProjectByTeamIdEventConsumer>();
                configure.AddRequestClient<TeamNameReceivedEvent>();
                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration.GetSection("AmqpConf")["Host"], config =>
                    {
                        config.Username(configuration.GetSection("AmqpConf")["Username"]!);
                        config.Password(configuration.GetSection("AmqpConf")["Password"]!);

                    });
                    configurator.ReceiveEndpoint(QueueConfigurations.Checkpoint_Api_AnalysisNotAvgTime_Identity, cnf => cnf.ConfigureConsumer<AnalysisNotAvgEventConsumer>(context));
                    configurator.ReceiveEndpoint(QueueConfigurations.Checkpoint_Api_ListProject_Identity, cnf => cnf.ConfigureConsumer<GetAllProjectByTeamIdEventConsumer>(context));

                });

            });
            services.AddHangfire(y => y.UseMemoryStorage());
            services.AddHangfireServer();
            services.AddOpenApi();
            return services;
        }
    }
}
