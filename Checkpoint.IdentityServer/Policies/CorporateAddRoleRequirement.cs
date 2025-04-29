using Microsoft.AspNetCore.Authorization;

namespace Checkpoint.IdentityServer.Policies
{
    public class CorporateAddRoleRequirement : IAuthorizationRequirement
    {
    }
    public class CorporateAddRole : AuthorizationHandler<CorporateAddRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CorporateAddRoleRequirement requirement)
        {
            var userClaims = context.User.Claims.ToList();
            throw new NotImplementedException();
        }
    }
}
