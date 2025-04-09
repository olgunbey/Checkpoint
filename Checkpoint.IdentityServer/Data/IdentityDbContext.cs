using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkpoint.IdentityServer.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
