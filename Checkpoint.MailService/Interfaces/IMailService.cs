
using Checkpoint.MailService.Dtos;

namespace Checkpoint.MailService.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string verificationCode);
        Task SendEmailApiAnalysisAsync(SendEmailApiAnalysisDto sendEmailApiAnalysisDto);
    }
}
