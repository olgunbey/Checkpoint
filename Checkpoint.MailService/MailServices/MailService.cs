using Checkpoint.MailService.Interfaces;
using System.Net.Mail;
using System.Text;

namespace Checkpoint.MailService.MailServices
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;
        public MailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }
        public void SendEmail(string toEmail, string subject)
        {
            using (MailMessage mailMessage = new())
            {
                mailMessage.From = new MailAddress("olgunbeysahin@gmail.com");
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("<h1>User Registered</h1>");

            }
            throw new NotImplementedException();
        }
    }
}
