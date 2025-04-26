using Microsoft.AspNetCore.Mvc.Filters;

namespace Checkpoint.IdentityServer.Filters
{
    public class IndividualFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
