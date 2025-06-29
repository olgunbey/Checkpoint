using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
            StringValues value = StringValues.Empty;

            httpContext.Request.Headers.TryGetValue("teamId", out value);
            httpContext.Request.Query.TryGetValue("teamId", out value);

            if (StringValues.IsNullOrEmpty(value))
            {
                await _next(httpContext);
            }

            if (user.Identity is { IsAuthenticated: true })
            {
                var userTeams = user.Claims.Single(y => y.Type == "teams");
                var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);
                CorporateJwtModel selectedGetTeam = deserializeData.Single(y => y.TeamId == short.Parse(value.ToString()));
                if (selectedGetTeam.Permissions.Any(y => y == Permission.Admin))
                {
                    httpContext.Items["AdminByPass"] = true;
                }

            }
            await _next(httpContext);
        }

    }
}
