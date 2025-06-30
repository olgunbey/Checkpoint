using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Dtos;

namespace Checkpoint.IdentityServer.Policies
{
    public class AddRoleRequirement : IAuthorizationRequirement
    {
    }
    public class AddRoleAuthorizationHandler(IHttpContextAccessor httpContext) : AuthorizationHandler<AddRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AddRoleRequirement requirement)
        {
            StringValues value = StringValues.Empty;
            var request = httpContext.HttpContext!.Request;
            httpContext.HttpContext.Items.TryGetValue("AdminByPass", out var adminCheck);

            if (adminCheck is true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            request.Query.TryGetValue("teamId", out value);


            var claims = context.User.Claims.ToList();
            var userTeams = claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtTeamModel>>(userTeams.Value);

            CorporateJwtTeamModel userGetSelectedTeamId = deserializeData.Single(y => y.TeamId == short.Parse(value.ToString()));


            if (userGetSelectedTeamId.Permissions.Any(permission => permission == Permission.Ekleme))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
