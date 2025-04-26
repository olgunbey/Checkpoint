using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Hash;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Checkpoint.IdentityServer.TokenServices
{
    public class TokenService(IdentityDbContext identityDbContext)
    {
        public Task<TokenResponseDto> CorporateToken(Corporate corporate, string issuer, string audience, string clientSecret)
        {
            var teams = corporate.UserTeams.Select(y => new
            {
                TeamId = y.TeamId,
                RoleId = y.UserTeamRoles.Single().RoleId,
                PermissionIds = y.UserTeamPermissions.Select(p => p.PermissionId).ToList()
            }).ToList();

            var teamsJson = JsonConvert.SerializeObject(teams);

            var claims = new List<Claim>
            {
                new Claim("teams", teamsJson)
            };



            DateTime expires = DateTime.UtcNow.AddMinutes(1);

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
                RefreshToken = "refresh",
                RefreshToken_LifeTime = DateTime.UtcNow.AddDays(5)
            });

        }
    }
}
