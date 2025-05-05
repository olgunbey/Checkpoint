using Checkpoint.MailService.Enums;

namespace Checkpoint.MailService.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(string toEmail, string subject, string verificationCode, SendEmailType sendEmailType);
    }
}
