using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Checkpoint.IdentityServer.Data
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();


            var optBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optBuilder.UseNpgsql(configuration.GetConnectionString("checkpoint"));
            return new IdentityDbContext(optBuilder.Options);
        }
    }
}
