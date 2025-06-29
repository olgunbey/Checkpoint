using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
                var userTeams = user.Claims.Single(y => y.Type == "teams");
                var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);

                bool isAdmin = bool.Parse(user.Claims.Single(y => y.Type == "IsAdmin").Value);
                if (isAdmin)
                {
                    httpContext.Items["AdminByPass"] = true;
                }

            }
            await _next(httpContext);
        }

    }
}
