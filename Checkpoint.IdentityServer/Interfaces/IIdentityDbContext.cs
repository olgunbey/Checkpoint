using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Entities.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Interfaces
{
    public interface IIdentityDbContext
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
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
