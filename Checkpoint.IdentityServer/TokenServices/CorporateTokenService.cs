using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Hash;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Checkpoint.IdentityServer.TokenServices
{
    public class CorporateTokenService(CorporateTransaction corporateTransaction, ClientTransaction clientTransaction)
    {
        public async Task<TokenResponseDto> GetToken(GetTokenRequestDto tokenRequestDto)
        {
            Client client = await clientTransaction.GetClient(tokenRequestDto.ClientId, tokenRequestDto.ClientSecret, tokenRequestDto.GrantType);

            Corporate corporate = await corporateTransaction.GetUser(tokenRequestDto);

            corporate = await corporateTransaction.CorporateSelectedRoleAndPermissionList(corporate);

            var corporateRoles = corporate.UserRoles.Select(y => y.Role.Name);

            var corporatePermissions = corporate.UserPermissions.Select(y => y.Permission.Name);

            var claims = FillClaims(corporateRoles).ToList();
            var permissions = FillPermissions(corporatePermissions);
            claims.AddRange(permissions);

            DateTime expires = DateTime.UtcNow.AddMinutes(1);

            var securityKey = new SymmetricSecurityKey(Hashing.Hash(client.ClientSecret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secretToken = new JwtSecurityToken(issuer: client.Issuer,
                audience: client.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(secretToken);

            return new TokenResponseDto()
            {
                AccessToken = token,
                AccessToken_LifeTime = expires,
                RefreshToken = "refresh",
                RefreshToken_LifeTime = DateTime.UtcNow.AddDays(5)
            };

        }

        private IEnumerable<Claim> FillClaims(IEnumerable<string> roles)
        {
            foreach (var item in roles)
            {
                yield return new Claim(ClaimTypes.Role, item);
            }
        }
        private IEnumerable<Claim> FillPermissions(IEnumerable<string> permissions)
        {
            foreach (var item in permissions)
            {
                yield return new Claim("Permissions", item);
            }
        }
    }
}
