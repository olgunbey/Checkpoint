using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.MailService.Data
{
    public class MailDbContext(DbContextOptions<MailDbContext> dbContextOptions) : DbContext(dbContextOptions), IMailDbContext
    {
        public DbSet<RegisterInbox> RegisterInbox { get; set; }
        public DbSet<NotSentMail> NotSentMail { get; set; }
    }
}
