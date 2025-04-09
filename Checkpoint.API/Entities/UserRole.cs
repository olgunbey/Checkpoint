using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class UserRole : BaseEntity
    {
        public int? IndividualId { get; set; }
        public int? CorporateId { get; set; }
        public Corporate? Corporate { get; set; }
        public Individual? Individual { get; set; }
    }
}
