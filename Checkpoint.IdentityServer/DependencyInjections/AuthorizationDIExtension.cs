using Checkpoint.IdentityServer.Policies;

namespace Checkpoint.IdentityServer.DependencyInjections
{
    public static class AuthorizationDIExtension
    {
        public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(configure =>
            {
                configure.AddPolicy("AddRole", configureBuilder =>
                configureBuilder.AddRequirements(new AddRoleRequirement()));
                configure.AddPolicy("AddCorporateToTeam", configureBuilder =>
                configureBuilder.AddRequirements(new AddCorporateToTeamRequirement()));
            });
            return services;
        }
    }
}
