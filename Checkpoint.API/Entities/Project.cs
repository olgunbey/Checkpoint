using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; }
        public ICollection<BaseUrl> BaseUrls { get; set; }
        public Corporate? Corporate { get; set; }
        public Individual? Individual { get; set; }
        public int? CorporateId { get; set; }
        public int? IndividualId { get; set; }
    }
}
