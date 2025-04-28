using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Interfaces;
using Checkpoint.IdentityServer.Services;
using Checkpoint.IdentityServer.TokenServices;

namespace Checkpoint.IdentityServer
{
    public static class DIExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<RegisterOutboxTransaction>();
            services.AddScoped<CorporateTransaction>();
            services.AddScoped<CompanyTransaction>();
            services.AddScoped<IIdentityDbContext, IdentityDbContext>();
            services.AddScoped<TokenService>();
            services.AddScoped<ClientTransaction>();
            services.AddScoped<UserServices>();
            services.AddSingleton<CorporateTokenDto>();
            return services;
        }
    }
}
