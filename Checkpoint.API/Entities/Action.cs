using Checkpoint.API.Common;
using Checkpoint.API.Enums;

namespace Checkpoint.API.Entities
{
    public class Action : BaseEntity
    {
        public string ActionPath { get; set; }
        public int ControllerId { get; set; }
        public Controller? Controller { get; set; }
        public RequestType RequestType { get; set; }
        public string? Body { get; set; }
        public string? Header { get; set; }
        public string? Query { get; set; }
    }
}
