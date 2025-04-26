using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Exceptions;
using Checkpoint.IdentityServer.Hash;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Checkpoint.IdentityServer.TokenServices
{
    public class CorporateTokenService(IdentityDbContext identityDbContext)
    {
        //public async Task<ResponseDto<TokenResponseDto>> GetToken(GetTokenRequestDto tokenRequestDto)
        //{

        //    corporate = await corporateTransaction.CorporateSelectedRoleAndPermissionList(corporate);

        //    var corporateRoles = corporate.UserRoles.Select(y => y.Role.Name);

        //    var corporatePermissions = corporate.UserPermissions.Select(y => y.Permission.Name);

        //    var claims = FillClaims(corporateRoles).ToList();
        //    var permissions = FillPermissions(corporatePermissions);
        //    claims.AddRange(permissions);

        //    DateTime expires = DateTime.UtcNow.AddMinutes(1);

        //    var securityKey = new SymmetricSecurityKey(Hashing.Hash(client.ClientSecret));

        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var secretToken = new JwtSecurityToken(issuer: client.Issuer,
        //        audience: client.Audience,
        //        claims: claims,
        //        expires: expires,
        //        signingCredentials: credentials
        //        );

        //    var token = new JwtSecurityTokenHandler().WriteToken(secretToken);

        //    var response = new TokenResponseDto()
        //    {
        //        AccessToken = token,
        //        AccessToken_LifeTime = expires,
        //        RefreshToken = "refresh",
        //        RefreshToken_LifeTime = DateTime.UtcNow.AddDays(5)
        //    };

        //    return ResponseDto<TokenResponseDto>.Success(response, 200);

        //}

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
        //bu kısım yazılacak
        public async Task<ResponseDto<NoContent>> Login(CorporateLoginDto loginDto)
        {
            Client? client = await identityDbContext.Client.FirstOrDefaultAsync(y => y.ClientId == loginDto.ClientId && y.ClientSecret == loginDto.ClientSecret && y.GrantType == loginDto.GrantType);

            if (client == null)
            {
                throw new NotFoundClientException("Client bulunamadı");
            }

            Corporate? hasCorporate = await identityDbContext.Corporate.FirstOrDefaultAsync(y => y.Mail == loginDto.Email && y.Password == loginDto.Password);

            if (hasCorporate == null)
            {
                throw new Exception("Yanlış kullanıcı adı veya şifre");
            }

            return ResponseDto<NoContent>.Success(204);
        }
    }
}
