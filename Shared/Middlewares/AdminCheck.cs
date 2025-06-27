using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Dtos;

namespace Shared.Middlewares
{
    public class AdminCheck
    {
        private readonly RequestDelegate _next;
        public AdminCheck(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var user = httpContext.User;
            var selectedTeamId = httpContext.Request.Headers["teamId"];

            if (user.Identity is { IsAuthenticated: true } && !string.IsNullOrEmpty(selectedTeamId))
            {
                var userTeams = user.Claims.Single(y => y.Type == "teams");
                var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);
                CorporateJwtModel selectedGetTeam = deserializeData.Single(y => y.TeamId == Int16.Parse(selectedTeamId!));
                if (selectedGetTeam.Permissions.Any(y => y == Permission.Admin))
                {
                    httpContext.Items["AdminByPass"] = true;
                }

            }
            await _next(httpContext);
        }

    }
}
