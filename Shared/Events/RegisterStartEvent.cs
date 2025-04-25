namespace Shared.Events
{
    public class RegisterStartEvent
    {
        public Guid CorrelationId { get; set; }
        public string Email { get; set; }
        public List<RegisterOutbox> RegisterOutboxes { get; set; }
    }
}
