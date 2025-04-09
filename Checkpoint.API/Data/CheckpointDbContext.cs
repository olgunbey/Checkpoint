using Checkpoint.API.Entities;
using Checkpoint.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkpoint.API.Data
{
    public sealed class CheckpointDbContext(DbContextOptions<CheckpointDbContext> dbContextOptions) : DbContext(dbContextOptions), IApplicationDbContext
    {
        public DbSet<Entities.Action> Action { get; set; }
        public DbSet<Controller> Controller { get; set; }
        public DbSet<BaseUrl> BaseUrl { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Corporate> Corporate { get; set; }
        public DbSet<Individual> Individual { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
