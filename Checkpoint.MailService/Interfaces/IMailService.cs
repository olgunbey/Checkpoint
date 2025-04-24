namespace Checkpoint.MailService.Interfaces
{
    public interface IMailService
    {
        void SendEmail(string toEmail, string subject);
    }
}
