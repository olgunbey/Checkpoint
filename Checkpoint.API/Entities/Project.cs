using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Project : BaseEntity
    {
        public required string ProjectName { get; set; }
        public ICollection<BaseUrl> BaseUrls { get; set; }
        public int TeamId { get; set; }
        public int? IndividualId { get; set; }
    }
}
