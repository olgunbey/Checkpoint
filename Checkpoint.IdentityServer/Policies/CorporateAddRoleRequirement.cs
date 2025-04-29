using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Checkpoint.IdentityServer.Policies
{
    //Burada corporate'ye rol ekleme yetkisi olanlar erişebilecek
    public class CorporateAddRoleRequirement : IAuthorizationRequirement
    {
    }
    public class CorporateAddRole : AuthorizationHandler<CorporateAddRoleRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CorporateAddRoleRequirement requirement)
        {
            var userClaims = context.User.Claims.ToList();
            var userTeam = context.User.Claims.Single(y => y.Value == "teams");

            var data = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeam.Value);
            Console.WriteLine("test123");
            return;
        }
    }
}
