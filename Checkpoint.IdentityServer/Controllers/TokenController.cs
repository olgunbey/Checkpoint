using Checkpoint.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequest)
        {
            return Ok();
        }
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
