using Checkpoint.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.API.ResponseHandler
{
    public class Response : ControllerBase
    {
        protected IActionResult Handlers<T>(ResponseDto<T> responseDto)
        {
            Response.StatusCode = responseDto.StatusCode;
            if (Response.StatusCode == 204)
                return new ObjectResult(null);

            return new ObjectResult(responseDto);

        }
    }
}
