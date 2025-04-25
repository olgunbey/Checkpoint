using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Checkpoint.IdentityServer
{
    public class BaseController : ControllerBase
    {
        protected IActionResult Handlers<T>(ResponseDto<T> responseDto)
        {
            HttpContext.Response.StatusCode = responseDto.StatusCode;
            if (HttpContext.Response.StatusCode == 204)
                return new ObjectResult(null);

            return new ObjectResult(responseDto);

        }
    }
}
