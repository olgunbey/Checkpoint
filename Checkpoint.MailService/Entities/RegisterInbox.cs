namespace Checkpoint.MailService.Entities
{
    public class RegisterInbox
    {
        public int Id { get; set; }
        public bool Processed { get; set; }
        public string Mail { get; set; }
        public required string CorporateName { get; set; }
        public string VerificationCode { get; set; }
        public string Password { get; set; }

    }
}
