using Microsoft.EntityFrameworkCore;

namespace Checkpoint.MailService.Data
{
    public class MailDbContext(DbContextOptions<MailDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {

    }
}
