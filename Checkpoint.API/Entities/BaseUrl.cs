using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class BaseUrl : BaseEntity
    {
        public required string BasePath { get; set; }
        public ICollection<RequestInfo> RequestInfos { get; set; }

        public ICollection<Controller> Controllers { get; set; }

    }
}
