using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public Role? Role { get; set; }
        public Permission? Permission { get; set; }
        public ICollection<Individual>? Individual { get; set; }
        public ICollection<Corporate>? Corporate { get; set; }
    }
}
