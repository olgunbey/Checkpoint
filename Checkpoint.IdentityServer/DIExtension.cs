using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Interfaces;
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
            services.AddScoped<CorporateTokenService>();
            services.AddScoped<ClientTransaction>();

            return services;
        }
    }
}
