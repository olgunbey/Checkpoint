using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Hash;

namespace Checkpoint.IdentityServer.Services
{
    public class UserServices(IIdentityDbContext identityDbContext, TokenService corporateTokenService)
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

            var responseToken = await corporateTokenService.CorporateToken(corporate.Entity, hasClient.Issuer, hasClient.Audience, hasClient.ClientSecret);

            return ResponseDto<TokenResponseDto>.Success(responseToken, 200);

        }

        public async Task<ResponseDto<NoContent>> AddRoleAsync(int teamId, string roleName, int userId)
        {
            identityDbContext.Role.Add(
                new Role()
                {
                    CreateUserId = userId,
                    Name = roleName,
                    TeamId = teamId,
                });

            await identityDbContext.SaveChangesAsync(CancellationToken.None);
            return ResponseDto<NoContent>.Success(204);
        }
    }
}
