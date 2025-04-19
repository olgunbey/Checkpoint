using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.MailService.Data
{
    public class MailInboxContext(DbContextOptions<MailInboxContext> dbContextOptions) : DbContext(dbContextOptions), IMailInboxDbContext
    {
        public DbSet<RegisterInbox> RegisterInbox { get; set; }
    }
}
