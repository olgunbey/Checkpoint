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
        public List<Body>? Body { get; set; }
        public List<Header>? Header { get; set; }
        public List<Query>? Query { get; set; }
    }
}
