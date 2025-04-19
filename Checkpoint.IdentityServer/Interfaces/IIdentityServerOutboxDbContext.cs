using Checkpoint.IdentityServer.Entities.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Interfaces
{
    public interface IIdentityServerOutboxDbContext
    {
        public DbSet<RegisterOutbox> RegisterOutbox { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
