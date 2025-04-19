namespace Checkpoint.MailService.Entities
{
    public class CorporateMail
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int CorporateId { get; set; }
    }
}
