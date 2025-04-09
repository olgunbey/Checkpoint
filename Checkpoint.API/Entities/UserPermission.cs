using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class UserPermission : BaseEntity
    {
        public int? IndividualId { get; set; }
        public Individual? Individual { get; set; }
        public int? CorporateId { get; set; }
        public Corporate? Corporate { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
