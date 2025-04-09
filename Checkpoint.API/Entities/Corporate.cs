using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Corporate : BaseEntity
    {
        public required string Mail { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserPermission> UserPermission { get; set; }

    }
}
