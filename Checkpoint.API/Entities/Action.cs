using Checkpoint.API.Common;
using Checkpoint.API.Enums;

namespace Checkpoint.API.Entities
{
    public class Action : BaseEntity
    {
        public required string ActionPath { get; set; }
        public int ControllerId { get; set; }
        public Controller? Controller { get; set; }
        public RequestType RequestType { get; set; }
    }
}
