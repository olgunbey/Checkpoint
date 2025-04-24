using MassTransit;

namespace Shared.Events
{
    public class RegisterOutboxEventBatch : CorrelatedBy<Guid>
    {
        public RegisterOutboxEventBatch(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public List<RegisterOutbox> RegisterOutboxes { get; set; }
    }
}
