namespace Checkpoint.API.Interfaces
{
    public interface IRequestPayload
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
