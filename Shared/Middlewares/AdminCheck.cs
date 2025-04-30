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

            if (user.Identity is { IsAuthenticated: true })
            {
                var userTeams = httpContext.User.Claims.Single(y => y.Type == "teams");
                var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);
                if (deserializeData.Any(y => y.Permissions.Any(y => y == Permission.Admin)))
                {
                    httpContext.Items["AdminByPass"] = true;
                }

            }
            await _next(httpContext);
        }

    }
}
