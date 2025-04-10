using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.Interfaces
{
    public interface IIdentityDbContext
    {
        public DbSet<Corporate> Corporate { get; set; }
        public DbSet<Individual> Individual { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Client> Client { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
