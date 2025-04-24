using MassTransit;

namespace Shared.Events
{
    public class MailSentEvent : CorrelatedBy<Guid>
    {
        public MailSentEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }

    }
}
