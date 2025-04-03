using Checkpoint.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkpoint.API.Data
{
    public sealed class CheckpointDbContext : DbContext, IApplicationDbContext
    {
        public CheckpointDbContext(DbContextOptions<CheckpointDbContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Entities.Action> Action { get; set; }
        public DbSet<Entities.Controller> Controller { get; set; }
        public DbSet<Entities.BaseUrl> BaseUrl { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
