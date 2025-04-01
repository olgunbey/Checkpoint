using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Controller : BaseEntity
    {
        public required string ControllerPath { get; set; }
        public int BaseUrlId { get; set; }
        public BaseUrl? BaseUrl { get; set; }
        public ICollection<Action>? Actions { get; set; }

    }
}
