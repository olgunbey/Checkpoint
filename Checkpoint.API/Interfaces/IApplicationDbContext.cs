using Checkpoint.API.Common;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.API.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Entities.Action> Action { get; set; }
        public DbSet<Entities.Controller> Controller { get; set; }
        public DbSet<Entities.BaseUrl> BaseUrl { get; set; }
        public DbSet<Entities.RequestInfo> RequestInfo { get; set; }
    }
}
