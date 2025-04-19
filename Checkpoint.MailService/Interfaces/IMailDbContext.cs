using Checkpoint.MailService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.MailService.Interfaces
{
    public interface IMailDbContext
    {
        public DbSet<CorporateMail> CorporateMail { get; set; }
    }
}
