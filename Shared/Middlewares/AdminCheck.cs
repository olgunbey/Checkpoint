using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
                if (user.Claims.Any(y => y.Type == ClaimTypes.Role && y.Value == "Admin"))
                {
                    httpContext.Items["AdminByPass"] = true;
                }
            }
            await _next(httpContext);
        }

    }
}
