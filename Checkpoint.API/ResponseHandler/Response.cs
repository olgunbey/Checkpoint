using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Checkpoint.API.ResponseHandler
{
    public class ApiResponseController : ControllerBase
    {
        protected IActionResult Handlers<T>(HttpContext httpContext, ResponseDto<T> responseDto)
        {
            httpContext.Response.StatusCode = responseDto.StatusCode;
            if (httpContext.Response.StatusCode == 204)
                return new ObjectResult(null);

            return new ObjectResult(responseDto);

        }
        protected IActionResult Handlers(HttpContext httpContext)
        {
            return new ObjectResult(null);

        }
    }
}
