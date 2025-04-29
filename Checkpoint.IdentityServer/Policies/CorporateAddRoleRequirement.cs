using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Shared.Constants;
using System.Security.Claims;

namespace Checkpoint.IdentityServer.Policies
{
    public class CorporateAddRoleRequirement : IAuthorizationRequirement
    {
    }
    public class CorporateAddRole(CorporateTokenDto corporateTokenDto, IHttpContextAccessor httpContext) : AuthorizationHandler<CorporateAddRoleRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CorporateAddRoleRequirement requirement)
        {
            var userTeams = context.User.Claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);

            if ((bool)httpContext.HttpContext!.Items["AdminByPass"]! == true)
            {
                corporateTokenDto.CorporateId = Int16.Parse(context.User.Claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                corporateTokenDto.CompanyId = Int16.Parse(context.User.Claims.Single(y => y.Type == "companyId").Value);
                context.Succeed(requirement);
            }
            else if (deserializeData.Any(y => y.Permissions.Any(x => x == Permission.Rol_Ekleme)))
            {
                corporateTokenDto.CorporateId = Int16.Parse(context.User.Claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                corporateTokenDto.CompanyId = Int16.Parse(context.User.Claims.Single(y => y.Type == "companyId").Value);
                context.Succeed(requirement);
            }
            return;
        }
    }
}
