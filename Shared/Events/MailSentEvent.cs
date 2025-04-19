namespace Shared.Events
{
    public class MailSentEvent
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string CompanyName { get; set; }
    }
}
