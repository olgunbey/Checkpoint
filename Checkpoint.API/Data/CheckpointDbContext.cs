using Microsoft.EntityFrameworkCore;

namespace Checkpoint.API.Data
{
    public sealed class CheckpointDbContext : DbContext
    {
        public CheckpointDbContext(DbContextOptions<CheckpointDbContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Entities.Action> Action { get; set; }
        public DbSet<Entities.Controller> Controller { get; set; }
        public DbSet<Entities.BaseUrl> BaseUrl { get; set; }
        public DbSet<Entities.RequestInfo> RequestInfo { get; set; }
    }
}
