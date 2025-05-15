using Checkpoint.IdentityServer.Constants;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Redis;
using Shared.Common;
using Shared.Hash;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;



namespace Checkpoint.IdentityServer.TokenServices
{
    public class TokenService(IIdentityDbContext identityDbContext, IRedisClientAsync redisClientAsync)
    {
        public Task<TokenResponseDto> CorporateTokenAsync(Corporate corporate, string issuer, string audience, string clientSecret)
        {
            var teams = corporate.UserTeams.Select(y => new
            {
                y.TeamId,
                Role = y.UserTeamRoles.Select(y => y.Role.Name).Single(),
                Permissions = y.UserTeamPermissions.Select(p => p.Permission.Name).ToList(),
            }).ToList();

            var teamsJson = JsonConvert.SerializeObject(teams);


            var claims = new List<Claim>
            {
                new Claim("teams", teamsJson),
                new Claim("companyId",corporate.CompanyId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,corporate.Id.ToString())
            };



            DateTime expires = DateTime.UtcNow.AddMinutes(30);

            var securityKey = new SymmetricSecurityKey(Hashing.Hash(clientSecret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secretToken = new JwtSecurityToken(issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(secretToken);

            return Task.FromResult(new TokenResponseDto()
            {
                AccessToken = token,
                AccessToken_LifeTime = expires,
                RefreshToken = GenerateRefreshToken(),
                RefreshToken_LifeTime = DateTime.UtcNow.AddDays(5)
            });

        }
        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        public async Task<ResponseDto<NoContent>> ControlRefreshTokenAsync(ControlRefreshTokenDto controlRefreshTokenDto)
        {
            var getAllRefreshToken = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey);

            if (getAllRefreshToken.Any(y => y.ValidityPeriod <= DateTime.UtcNow))
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("Refresh token bulunamadı veya geçersiz", 400);
        }
        public async Task<ResponseDto<NoContent>> RemoveRefreshTokenAsync(int userId)
        {
            var refreshTokens = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey);

            var removeRefreshToken = refreshTokens.Single(y => y.UserId == userId);

            refreshTokens.Remove(removeRefreshToken);

            await redisClientAsync.SetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey, refreshTokens);

            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<TokenResponseDto>> GenerateAccessTokenAsync(GenerateAccessTokenDto generateAccessTokenDto)
        {
            var cacheRefreshTokenList = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey);

            var getRefreshTokenDto = cacheRefreshTokenList.FirstOrDefault(y => y.UserId == generateAccessTokenDto.UserId);

            if (getRefreshTokenDto == null)
                return ResponseDto<TokenResponseDto>.Fail("kullanıcı bulunamadı", 400);


            if (getRefreshTokenDto.RefreshToken != getRefreshTokenDto.RefreshToken || getRefreshTokenDto.ValidityPeriod != DateTime.UtcNow)
                return ResponseDto<TokenResponseDto>.Fail("Süresi bitmiş", 401);

            Corporate? corporate = await identityDbContext.Corporate.FindAsync(generateAccessTokenDto.UserId);

            if (corporate == null)
                return ResponseDto<TokenResponseDto>.Fail("Kullanıcı bulunamadı", 400);

            var getClient = (await identityDbContext.Client.FindAsync(1))!;

            var generateToken = await CorporateTokenAsync(corporate, getClient.Issuer, getClient.Audience, getClient.ClientSecret);

            getRefreshTokenDto.RefreshToken = generateToken.RefreshToken;
            getRefreshTokenDto.ValidityPeriod = generateToken.RefreshToken_LifeTime;

            await redisClientAsync.SetAsync(IdentityServerConstants.RedisRefreshTokenKey, cacheRefreshTokenList);

            return ResponseDto<TokenResponseDto>.Success(generateToken, 200);


        }
    }
}
