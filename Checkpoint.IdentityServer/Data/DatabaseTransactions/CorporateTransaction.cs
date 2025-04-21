using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class CorporateTransaction(IIdentityDbContext identityDbContext)
    {
        public async Task<Corporate> GetUser(GetTokenRequestDto getTokenRequestDto)
        {
            Corporate? hasCorporate = await identityDbContext.Corporate.FirstOrDefaultAsync(y => y.Mail == getTokenRequestDto.Email && y.Password == getTokenRequestDto.Password);

            if (hasCorporate == null)
            {
                throw new Exception("Yanlış kullanıcı adı veya şifre");
            }
            return hasCorporate;
        }
        public async Task<Corporate> CorporateSelectedRoleAndPermissionList(Corporate corporate)
        {
            var entryCorporate = identityDbContext.Corporate.Entry(corporate);

            var taskPermission = entryCorporate
                .Collection(y => y.UserPermissions)
                .Query()
                .Include(y => y.Permission)
                .LoadAsync();

            var taskRole = entryCorporate
                .Collection(y => y.UserRoles)
                .Query()
                .Include(y => y.Role)
                .LoadAsync();

            await Task.WhenAll(taskPermission, taskRole);
            return corporate;

        }

        public async Task AddCorporate(Corporate corporate, CancellationToken cancellationToken)
        {
            identityDbContext.Corporate.Add(corporate);
            await identityDbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task CorporateAddRange(IEnumerable<Corporate> corporate, CancellationToken cancellationToken)
        {
            identityDbContext.Corporate.AddRange(corporate);
            await identityDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
