using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using Checkpoint.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class CorporateTransaction
    {
        private readonly IIdentityDbContext _identityDbContext;
        public CorporateTransaction(IIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public async Task<Corporate> GetUser(GetTokenRequestDto getTokenRequestDto)
        {
            Corporate? hasCorporate = await _identityDbContext.Corporate.FirstOrDefaultAsync(y => y.Mail == getTokenRequestDto.Email && y.Password == getTokenRequestDto.Password);

            if (hasCorporate == null)
            {
                throw new Exception("Yanlış kullanıcı adı veya şifre");
            }
            return hasCorporate;
        }
        public async Task<Corporate> CorporateSelectedRoleAndPermissionList(Corporate corporate)
        {
            var taskPermission = _identityDbContext.Corporate.Entry(corporate)
                .Collection(y => y.UserPermissions)
                .Query()
                .Include(y => y.Permission)
                .LoadAsync();

            var taskRole = _identityDbContext.Corporate.Entry(corporate)
                .Collection(y => y.UserRoles)
                .Query()
                .Include(y => y.Role)
                .LoadAsync();

            await Task.WhenAll(taskPermission, taskRole);
            return corporate;

        }
    }
}
