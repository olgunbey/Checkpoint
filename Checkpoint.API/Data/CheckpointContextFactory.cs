using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Checkpoint.API.Data
{
    public class CheckpointContextFactory : IDesignTimeDbContextFactory<CheckpointDbContext>
    {
        public CheckpointDbContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<CheckpointDbContext>();
            dbContextOptions.UseNpgsql("Host=localhost;Username=postgres;Password=checkpointpassword;Port=5432;Database=CheckpointDb");
            return new CheckpointDbContext(dbContextOptions.Options);

        }
    }
}
