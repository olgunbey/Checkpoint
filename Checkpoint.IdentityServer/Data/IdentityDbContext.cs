using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkpoint.IdentityServer.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Corporate> Corporate { get; set; }
        public DbSet<Individual> Individual { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Company> Company { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
