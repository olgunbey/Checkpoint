using Checkpoint.IdentityServer.Constants;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    public class TokenService(IIdentityDbContext identityDbContext, IRedisClientAsync redisClientAsync, IOptions<TokenConf> tokenConf)
    {
        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        private Task<TokenResponseDto> CreateTokenAsync(IEnumerable<Claim> claims)
        {
            DateTime accessTokenExpire = DateTime.UtcNow.AddMinutes(30);
            DateTime refreshTokenExpire = DateTime.UtcNow.AddDays(3);

            string clientSecret = tokenConf.Value.ClientSecret;
            string issuer = tokenConf.Value.Issuer;
            string audience = tokenConf.Value.Audience;

            var securityKey = new SymmetricSecurityKey(Hashing.Hash(clientSecret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secretToken = new JwtSecurityToken(issuer: issuer,
                audience: audience,
                claims: claims,
                expires: accessTokenExpire,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(secretToken);

            return Task.FromResult(new TokenResponseDto
            {
                AccessToken = token,
                AccessToken_LifeTime = accessTokenExpire,
                RefreshToken = GenerateRefreshToken(),
                RefreshToken_LifeTime = refreshTokenExpire,
            });
        }
        public async Task<TokenResponseDto> CorporateTokenAsync(Corporate corporate)
        {
            var teams = corporate.UserTeams.Select(y => new
            {
                y.TeamId,
                Role = y.UserTeamRoles.Select(y => y.Role.Name).SingleOrDefault(),
                Permissions = y.UserTeamPermissions.Select(p => p.Permission.Name).ToList(),
            }).ToList();

            var teamsJson = JsonConvert.SerializeObject(teams);

            var claims = new List<Claim>
            {
                new Claim("teams", teamsJson),
                new Claim("companyId",corporate.CompanyId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,corporate.Id.ToString())
            };

            if (corporate.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            var responseTokenDto = await CreateTokenAsync(claims);
            return responseTokenDto;

        }

        public async Task<ResponseDto<TokenResponseDto>> ControlRefreshTokenAsync(ControlRefreshTokenDto controlRefreshTokenDto)
        {
            var getAllRefreshToken = await redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>(IdentityServerConstants.RedisRefreshTokenKey);

            var getRefreshToken = getAllRefreshToken.FirstOrDefault(y => y.RefreshToken == controlRefreshTokenDto.RefreshToken);

            if (getRefreshToken == null || getRefreshToken.ValidityPeriod <= DateTime.UtcNow)
                return ResponseDto<TokenResponseDto>.Fail("Refresh token bulunamadı veya geçersiz", 401);

            int userId = getRefreshToken.UserId;

            var corporateUser = (await identityDbContext.Corporate.FindAsync(userId))!;

            await identityDbContext.Corporate.Entry(corporateUser).Collection(y => y.UserTeams)
            .Query()
            .Include(y => y.UserTeamRoles)
            .ThenInclude(y => y.Role)
            .Include(y => y.UserTeamPermissions)
            .ThenInclude(y => y.Permission)
             .LoadAsync();


            var response = await CorporateTokenAsync(corporateUser);

            getAllRefreshToken.Remove(getRefreshToken);

            getAllRefreshToken.Add(new CacheRefreshTokenDto
            {
                RefreshToken = response.RefreshToken,
                UserId = userId,
                ValidityPeriod = DateTime.UtcNow
            });

            await redisClientAsync.SetAsync(IdentityServerConstants.RedisRefreshTokenKey, getAllRefreshToken);

            return ResponseDto<TokenResponseDto>.Success(response, 200);
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

            var getRefreshTokenDto = cacheRefreshTokenList.FirstOrDefault(y => y.RefreshToken == generateAccessTokenDto.RefreshToken);

            if (getRefreshTokenDto == null)
                return ResponseDto<TokenResponseDto>.Fail("kullanıcı bulunamadı", 400);


            if (getRefreshTokenDto.ValidityPeriod <= DateTime.UtcNow)
                return ResponseDto<TokenResponseDto>.Fail("Süresi bitmiş", 401);

            Corporate? corporate = await identityDbContext.Corporate.FindAsync(getRefreshTokenDto.UserId);

            if (corporate == null)
                return ResponseDto<TokenResponseDto>.Fail("Kullanıcı bulunamadı", 400);

            await identityDbContext.Corporate.Entry(corporate).Collection(y => y.UserTeams)
                .Query()
                .Include(y => y.UserTeamRoles)
                .ThenInclude(y => y.Role)
                .Include(y => y.UserTeamPermissions)
                .ThenInclude(y => y.Permission).LoadAsync();

            var generateToken = await CorporateTokenAsync(corporate);

            getRefreshTokenDto.RefreshToken = generateToken.RefreshToken;
            getRefreshTokenDto.ValidityPeriod = generateToken.RefreshToken_LifeTime;

            await redisClientAsync.SetAsync(IdentityServerConstants.RedisRefreshTokenKey, cacheRefreshTokenList);

            return ResponseDto<TokenResponseDto>.Success(generateToken, 200);


        }
    }
}
