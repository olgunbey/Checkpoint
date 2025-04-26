using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Entities.Outbox;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkpoint.IdentityServer.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions) : DbContext(dbContextOptions), IIdentityDbContext
    {
        public DbSet<Corporate> Corporate { get; set; }
        public DbSet<Individual> Individual { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserTeam> UserTeam { get; set; }
        public DbSet<UserTeamPermission> UserTeamPermission { get; set; }
        public DbSet<UserTeamRole> UserTeamRole { get; set; }
        public DbSet<CompanyPermission> CompanyPermission { get; set; }
        public DbSet<CompanyRole> CompanyRole { get; set; }
        public DbSet<UserTeam> Team { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<RegisterOutbox> RegisterOutbox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
