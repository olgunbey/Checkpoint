namespace Checkpoint.API.Interfaces
{
    public interface IRequestPayload
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Enums.ValueType ValueType { get; set; }
    }
}
