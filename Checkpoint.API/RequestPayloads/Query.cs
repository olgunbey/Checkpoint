using Checkpoint.API.Interfaces;

namespace Checkpoint.API.RequestPayloads
{
    public class Query : IRequestPayload
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Enums.ValueType ValueType { get; set; }
    }
}
