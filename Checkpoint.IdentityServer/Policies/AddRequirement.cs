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
            if (string.IsNullOrEmpty(httpContext.HttpContext!.Request.Headers["teamId"]))
            {
                return;
            }


            var selectedTeamId = Int16.Parse(httpContext.HttpContext!.Request.Headers["teamId"]!);
            var claims = context.User.Claims.ToList();
            var userTeams = claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);

            CorporateJwtModel userGetSelectedTeamId = deserializeData.Single(y => y.TeamId == selectedTeamId);
            if ((bool)httpContext.HttpContext!.Items["AdminByPass"]! == true)
            {
                tokenDto.CorporateId = Int16.Parse(claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                tokenDto.CompanyId = Int16.Parse(claims.Single(y => y.Type == "companyId").Value);
                tokenDto.SelectedTeamId = selectedTeamId;
                context.Succeed(requirement);
            }
            else if (userGetSelectedTeamId.Permissions.Any(permission => permission == Permission.Ekleme))
            {
                tokenDto.CorporateId = Int16.Parse(claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
                tokenDto.CompanyId = Int16.Parse(claims.Single(y => y.Type == "companyId").Value);
                tokenDto.SelectedTeamId = selectedTeamId;
                context.Succeed(requirement);
            }
            return;
        }
    }
}
