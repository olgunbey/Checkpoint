using Carter;
using Checkpoint.API.Consumers;
using Checkpoint.API.Data;
using Checkpoint.API.Dtos;
using Checkpoint.API.Features.Project.Command;
using Checkpoint.API.Interfaces;
using EventStore.Client;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Shared;
using Shared.Events;
using Shared.Hash;

namespace Checkpoint.API.DependencyInjections
{
    public static class GeneralDIExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddHangfire(y => y.UseMemoryStorage());
            services.AddHangfireServer();
            services.AddOpenApi();
            services.AddCarter();
            services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "https://localhost:7253",
                    ValidAudience = "https://localhost:5000",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Hashing.Hash("checkpointsecret"))
                };
            });

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("checkpoint"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, AddProject.AuthorizationTransaction.Handler>();
            services.AddDbContext<CheckpointDbContext>(u => u.UseNpgsql(dataSource));
            services.AddScoped<IApplicationDbContext, CheckpointDbContext>();
            services.AddSingleton<CorporateTokenInformationDto>();
            services.AddSingleton(config =>
            {
                EventStoreClientSettings clientSettings = EventStoreClientSettings.Create(configuration.GetSection("EventStore")["db"]);
                var eventStoreClient = new EventStoreClient(clientSettings);
                return eventStoreClient;
            });

            services.AddMassTransit<IBus>(configure =>
            {
                configure.AddConsumer<TeamNameReceivedConsumer>();
                configure.AddRequestClient<GetAllProjectByTeamIdEvent>();
                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration.GetSection("AmqpConf")["Host"], config =>
                    {
                        config.Username(configuration.GetSection("AmqpConf")["Username"]!);
                        config.Password(configuration.GetSection("AmqpConf")["Password"]!);

                    });
                    configurator.ReceiveEndpoint(QueueConfigurations.Identity_Server_TeamNameReceived_Checkpoint_Api, cnf => cnf.ConfigureConsumer<TeamNameReceivedConsumer>(context));

                });

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

            return services;
        }
    }
}
