using Checkpoint.IdentityServer.Entities.Outbox;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Data
{
    public class IdentityServerOutboxDbContext(DbContextOptions<IdentityServerOutboxDbContext> dbContextOptions) : DbContext(dbContextOptions), IIdentityServerOutboxDbContext
    {
        public DbSet<RegisterOutbox> RegisterOutbox { get; set; }
    }
}
