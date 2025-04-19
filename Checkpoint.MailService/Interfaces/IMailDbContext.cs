using Checkpoint.MailService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.MailService.Interfaces
{
    public interface IMailDbContext
    {
        public DbSet<RegisterInbox> RegisterInbox { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
