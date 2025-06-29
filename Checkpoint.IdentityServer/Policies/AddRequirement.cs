using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Dtos;

namespace Checkpoint.IdentityServer.Policies
{
    public class AddRequirement : IAuthorizationRequirement
    {
    }
    public class Add(IHttpContextAccessor httpContext) : AuthorizationHandler<AddRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AddRequirement requirement)
        {

            StringValues value = StringValues.Empty;

            httpContext.HttpContext!.Request.Query.TryGetValue("teamId", out value);

            if (String.IsNullOrEmpty(value))
            {
                httpContext.HttpContext!.Request.Headers.TryGetValue("teamId", out value);
            }

            if (StringValues.IsNullOrEmpty(value))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var claims = context.User.Claims.ToList();
            var userTeams = claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);

            CorporateJwtModel userGetSelectedTeamId = deserializeData.Single(y => y.TeamId == short.Parse(value.ToString()));

            httpContext.HttpContext!.Items.TryGetValue("AdminByPass", out var adminCheck);


            if (adminCheck is true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else if (userGetSelectedTeamId.Permissions.Any(permission => permission == Permission.Ekleme))
            {

                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
