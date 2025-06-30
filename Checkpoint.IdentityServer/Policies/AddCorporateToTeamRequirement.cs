using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Shared;
using Shared.Constants;
using Shared.Dtos;

namespace Checkpoint.IdentityServer.Policies
{
    public class AddCorporateToTeamRequirement : IAuthorizationRequirement
    {
    }
    public class AddCorporateToTeamHandler(IHttpContextAccessor httpContext) : AuthorizationHandler<AddCorporateToTeamRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AddCorporateToTeamRequirement requirement)
        {
            var request = httpContext.HttpContext!.Request;
            httpContext.HttpContext.Items.TryGetValue("AdminByPass", out var adminCheck);

            if (adminCheck is true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            request.Query.TryGetValue("teamId", out StringValues value);

            var teamParsed = TokenTeamParsed.GetJwtTeamModel(context.User);

            CorporateJwtTeamModel userGetSelectedTeamId = teamParsed.Single(y => y.TeamId == short.Parse(value.ToString()));

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
