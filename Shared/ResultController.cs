using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Shared
{
    public class ResultController : ControllerBase
    {
        protected IActionResult Handlers<T>(HttpContext httpContext, ResponseDto<T> responseDto)
        {
            httpContext.Response.StatusCode = responseDto.StatusCode;
            if (httpContext.Response.StatusCode == 204)
                return new ObjectResult(null);

            return new ObjectResult(responseDto);
        }
    }
}
