using Checkpoint.IdentityServer.Constants;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Redis;
using Shared.Common;
using Shared.Hash;

namespace Checkpoint.IdentityServer.Services
{
    public class UserService(IIdentityDbContext identityDbContext, TokenService corporateTokenService, IRedisClientAsync redisClientAsync)
    {
        public async Task<ResponseDto<TokenResponseDto>> LoginAsync(CorporateLoginDto corporateLoginDto)
        {
            var hasClient = await identityDbContext.Client.FirstOrDefaultAsync(y => y.ClientId == corporateLoginDto.ClientId && y.ClientSecret == corporateLoginDto.ClientSecret);

            if (hasClient == null)
            {
                return ResponseDto<TokenResponseDto>.Fail("not found client", 404);
            }

            Corporate? hasCorporate = await identityDbContext.Corporate.FirstOrDefaultAsync(y => y.Mail == corporateLoginDto.Email && y.Password == Hashing.HashPassword(corporateLoginDto.Password));

            if (hasCorporate == null)
            {
                return ResponseDto<TokenResponseDto>.Fail("not found corporate", 404);
            }

            var corporate = identityDbContext.Corporate.Entry(hasCorporate);

            await corporate.Collection(y => y.UserTeams)
                .Query()
                .Include(y => y.UserTeamRoles)
                .ThenInclude(y => y.Role)
                .Include(y => y.UserTeamPermissions)
                .ThenInclude(y => y.Permission)
                 .LoadAsync();

            var responseToken = await corporateTokenService.CorporateTokenAsync(corporate.Entity);

            CacheRefreshTokenDto cacheRefreshTokenDto = new()
            {
                RefreshToken = responseToken.RefreshToken,
                UserId = corporate.Entity.Id,
                ValidityPeriod = responseToken.RefreshToken_LifeTime
            };
            var getRedisRefreshToken = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey);

            List<CacheRefreshTokenDto> refreshTokenDtos = new();
            if (getRedisRefreshToken != null)
            {
                var getRedisRefresh = getRedisRefreshToken.FirstOrDefault(y => y.UserId == corporate.Entity.Id);
                if (getRedisRefresh is not null)
                {
                    getRedisRefreshToken.Remove(getRedisRefresh);
                }
                refreshTokenDtos = getRedisRefreshToken;
            }
            refreshTokenDtos.Add(cacheRefreshTokenDto);
            await redisClientAsync.SetAsync(IdentityServerConstants.RedisRefreshTokenKey, refreshTokenDtos);
            return ResponseDto<TokenResponseDto>.Success(responseToken, 200);

        }

        public async Task<ResponseDto<NoContent>> AddRoleAsync(int teamId, string roleName)
        {
            identityDbContext.Role.Add(
                new Role()
                {
                    Name = roleName,
                    TeamId = teamId,
                });

            await identityDbContext.SaveChangesAsync(CancellationToken.None);
            return ResponseDto<NoContent>.Success(204);
        }
    }
}
