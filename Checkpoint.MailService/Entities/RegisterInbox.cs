namespace Checkpoint.MailService.Entities
{
    public class RegisterInbox
    {
        public int Id { get; set; }
        public bool Processed { get; set; }
        public string Mail { get; set; }
        public string CorporateName { get; set; }

    }
}
