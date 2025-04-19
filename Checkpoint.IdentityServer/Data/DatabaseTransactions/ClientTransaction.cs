using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Exceptions;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class ClientTransaction(IIdentityDbContext identityDbContext)
    {

        public async Task<Client> GetClient(string clientId, string clientSecret, string grantType)
        {
            Client? client = await identityDbContext.Client.FirstOrDefaultAsync(y => y.ClientId == clientId && y.ClientSecret == clientSecret && y.GrantType == grantType);

            if (client == null)
            {
                throw new NotFoundClientException("Client bulunamadı");
            }
            return client;

        }
    }
}
