using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Individual : BaseEntity
    {
        public required string Email { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
