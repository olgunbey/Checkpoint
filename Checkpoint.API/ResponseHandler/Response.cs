using Checkpoint.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.API.ResponseHandler
{
    public class ApiResponseController : ControllerBase
    {
        protected IActionResult Handlers<T>(ResponseDto<T> responseDto,HttpContext httpContext)
        {
            httpContext.Response.StatusCode = responseDto.StatusCode;
            if (httpContext.Response.StatusCode == 204)
                return new ObjectResult(null);

            return new ObjectResult(responseDto);

        }
    }
}
