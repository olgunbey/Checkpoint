namespace Checkpoint.MailService.Entities
{
    public class NotSentMail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public bool Processed { get; set; }
    }
}
