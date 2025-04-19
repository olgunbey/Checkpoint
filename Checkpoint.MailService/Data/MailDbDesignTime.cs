using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Checkpoint.MailService.Data
{
    public class MailDbDesignTime : IDesignTimeDbContextFactory<MailDbContext>
    {
        public MailDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            DbContextOptionsBuilder<MailDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<MailDbContext>();

            dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("checkpoint"));

            return new MailDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
