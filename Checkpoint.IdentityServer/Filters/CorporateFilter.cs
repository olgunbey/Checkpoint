using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Checkpoint.IdentityServer.Filters
{
    public class CorporateFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var corporateTokenDto = context.HttpContext.RequestServices.GetRequiredService<CorporateTokenDto>();
            var userClams = context.HttpContext.User.Claims.ToList();
            throw new NotImplementedException();
        }
    }
}
