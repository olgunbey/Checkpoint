using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(TokenService tokenService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> RemoveRefreshToken([FromQuery] int userId)
        {
            var responseDto = await tokenService.RemoveRefreshTokenAsync(userId);
            return Handlers(responseDto);
        }
        [HttpPost]
        public async Task<IActionResult> AccessTokenRefreshWithRefreshToken([FromBody] GenerateAccessTokenDto generateAccessTokenDto)
        {
            var responseDto = await tokenService.GenerateAccessTokenAsync(generateAccessTokenDto);
            return Handlers(responseDto);
        }
    }
}
