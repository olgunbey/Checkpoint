using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Permission : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<RolePermission>? RolePermission { get; set; }
        public ICollection<UserPermission>? UserPermission { get; set; }
    }
}
