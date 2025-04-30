using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shared.Hash;
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
    }
}
