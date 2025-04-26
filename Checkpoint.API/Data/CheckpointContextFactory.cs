using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Checkpoint.API.Data
{
    public class CheckpointContextFactory : IDesignTimeDbContextFactory<CheckpointDbContext>
    {
        public CheckpointDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var dbContextOptions = new DbContextOptionsBuilder<CheckpointDbContext>();
            dbContextOptions.UseNpgsql(configuration.GetConnectionString("checkpoint"));
            return new CheckpointDbContext(dbContextOptions.Options);

        }
    }
}
