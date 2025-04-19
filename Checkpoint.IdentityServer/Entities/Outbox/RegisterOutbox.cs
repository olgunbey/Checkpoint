namespace Checkpoint.IdentityServer.Entities.Outbox
{
    public class RegisterOutbox
    {
        public int Id { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string Payload { get; set; }
        public string EventType { get; set; }
    }
}
