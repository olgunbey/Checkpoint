using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Dtos;
using System.Security.Claims;

namespace Checkpoint.IdentityServer.Policies
{
    public class AddRequirement : IAuthorizationRequirement
    {
    }
    public class Add(IHttpContextAccessor httpContext, TokenDto tokenDto) : AuthorizationHandler<AddRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AddRequirement requirement)
        {
            var userTeams = context.User.Claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);

            if ((bool)httpContext.HttpContext!.Items["AdminByPass"]! == true)
            {
                tokenDto.CorporateId = Int16.Parse(context.User.Claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                tokenDto.CompanyId = Int16.Parse(context.User.Claims.Single(y => y.Type == "companyId").Value);
                context.Succeed(requirement);
            }
            else if (deserializeData.Any(y => y.Permissions.Any(x => x == Permission.Ekleme)))
            {
                tokenDto.CorporateId = Int16.Parse(context.User.Claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                tokenDto.CompanyId = Int16.Parse(context.User.Claims.Single(y => y.Type == "companyId").Value);
                context.Succeed(requirement);
            }
            return;
        }
    }
}
