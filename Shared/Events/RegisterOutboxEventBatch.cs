namespace Shared.Events
{
    public class RegisterOutboxEventBatch
    {
        public List<RegisterOutboxEvent> Events { get; set; }
    }
}
