using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(IRedisClientAsync redisClientAsync, TokenService tokenService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> RemoveRefreshToken([FromQuery] int userId)
        {
            var responseDto = await tokenService.RemoveRefreshTokenAsync(userId);
            return Handlers(responseDto);
        }
        [HttpPost]
        public async Task<IActionResult> ControlRefreshToken([FromBody] ControlRefreshTokenDto refreshToken)
        {
            var responseDto = await tokenService.ControlRefreshTokenAsync(refreshToken);
            return Handlers(responseDto);

        }
        [HttpPost]
        public async Task<IActionResult> GenerateAccessToken([FromBody] GenerateAccessTokenDto generateAccessTokenDto)
        {
            var responseDto = await tokenService.GenerateAccessTokenAsync(generateAccessTokenDto);
            return Handlers(responseDto);
        }
    }
}
