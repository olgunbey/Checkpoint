using Checkpoint.API.Common;
using Checkpoint.API.Enums;
using Checkpoint.API.RequestPayloads;

namespace Checkpoint.API.Entities
{
    public class Action : BaseEntity
    {
        public string ActionPath { get; set; }
        public int ControllerId { get; set; }
        public Controller? Controller { get; set; }
        public RequestType RequestType { get; set; }
        public Body? Body { get; set; }
        public Header? Header { get; set; }
        public Query? Query { get; set; }
    }
}
