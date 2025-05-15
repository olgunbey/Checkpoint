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
            //var refreshTokens = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>("refresh-token");

            //var removeRefreshToken = refreshTokens.Single(y => y.UserId == userId);

            //refreshTokens.Remove(removeRefreshToken);

            //await redisClientAsync.SetAsync<List<CacheRefreshTokenDto>>("refresh-token", refreshTokens);

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
        public async Task<IActionResult> GenerateAccessToken([FromBody] string refreshToken)
        {
            //accesstoken üretmek için bir refresh token alınır. Bu refresh tokene göre yeni bir access token üretilir
            //ve refres token güncellenir
            return Ok();
        }
    }
}
