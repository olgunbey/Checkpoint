using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
