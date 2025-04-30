using Checkpoint.API.Interfaces;

namespace Checkpoint.API.RequestPayloads
{
    public class Query : IRequestPayload
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
