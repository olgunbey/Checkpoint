using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(IRedisClientAsync redisClientAsync) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> RemoveRefreshToken([FromQuery] int userId)
        {
            var refreshTokens = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>("refresh-token");

            var removeRefreshToken = refreshTokens.Single(y => y.UserId == userId);

            refreshTokens.Remove(removeRefreshToken);

            await redisClientAsync.SetAsync<List<CacheRefreshTokenDto>>("refresh-token", refreshTokens);

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> GenerateRefreshToken([FromBody] string refreshToken)
        {
            var refreshTokens = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>("refresh-token");

            if (refreshTokens.Any(y => y.RefreshToken == refreshToken && y.ValidityPeriod <= DateTime.UtcNow))
            {
                return NoContent();
            }
            return Unauthorized();
        }
    }
}
