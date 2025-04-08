using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class BaseUrl : BaseEntity
    {
        public string BasePath { get; set; }

        public ICollection<Controller> Controllers { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
