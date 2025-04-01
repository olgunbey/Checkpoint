using Checkpoint.API.Common;

namespace Checkpoint.API.Entities
{
    public class Action : BaseEntity
    {
        public required string ActionPath { get; set; }
        public int ControllerId { get; set; }
        public Controller? Controller { get; set; }
    }
}
