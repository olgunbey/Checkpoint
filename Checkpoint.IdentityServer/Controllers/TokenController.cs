using Checkpoint.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequest)
        {
            return Ok();
        }
    }
}
