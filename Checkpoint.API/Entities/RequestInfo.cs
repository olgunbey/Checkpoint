using Checkpoint.API.Common;
using Checkpoint.API.Enums;

namespace Checkpoint.API.Entities
{
    public class RequestInfo : BaseEntity
    {
        public required BaseUrl BaseUrl { get; set; }
        public int BaseUrlId { get; set; }
        public RequestType RequestType { get; set; }
        public string? Body { get; set; }
        public string? Header { get; set; }
        public string? Query { get; set; }

    }
}
