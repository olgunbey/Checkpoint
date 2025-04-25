using MassTransit;

namespace Shared.Events
{
    public class RegisterOutboxEvent : CorrelatedBy<Guid>
    {
        public RegisterOutboxEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public List<RegisterOutbox> RegisterOutboxes { get; set; }
    }
}
