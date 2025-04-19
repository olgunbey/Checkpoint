using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class CompanyTransaction(IdentityDbContext identityDbContext)
    {
        public async Task<Company?> GetCompanyByCompanyKey(string key)
        {
            return await identityDbContext.Company.FirstOrDefaultAsync(y => y.Key == key);
        }
        public async Task<Company?> GetCompanyByCompanyName(string companyName)
        {
            return await identityDbContext.Company.FirstOrDefaultAsync(y => y.Name == companyName);
        }
    }
}
