using Checkpoint.MailService.Dtos;
using Checkpoint.MailService.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Checkpoint.MailService.MailServices
{
    public class MailService(SmtpClient smtpClient, IOptions<MailInformation> mailInformation) : IMailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string verificationCode)
        {
            using (MimeMessage mailMessage = new())
            {
                mailMessage.Subject = subject;
                mailMessage.From.Add(MailboxAddress.Parse(mailInformation.Value.Username));
                mailMessage.To.Add(MailboxAddress.Parse(toEmail));

                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"<!DOCTYPE html>
<html lang=""tr"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Giriş Kodu</title>
    <style>
        @media screen and (max-width: 600px) {{
            .content {{
                width: 100% !important;
                padding: 20px !important;
            }}

            .code-box {{
                font-size: 28px !important;
            }}
        }}
    </style>
</head>

<body style=""margin: 0; padding: 0; font-family: 'Poppins', Arial, sans-serif; background-color: #f9f9f9;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f9f9f9;"">
        <tr>
            <td align=""center"" style=""padding: 30px;"">
                <table class=""content"" width=""600"" cellpadding=""0"" cellspacing=""0""
                    style=""background: #ffffff; border-radius: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.05); padding: 40px; text-align: center;"">

                    <tr>
                        <td style=""font-size: 22px; font-weight: 600; color: #1A2238; padding-bottom: 10px;"">
                            Giriş Kodunuz
                        </td>
                    </tr>

                    <tr>
                        <td style=""font-size: 16px; color: #444; padding-bottom: 20px;"">
                            Güvenliğiniz için, giriş yapmak üzere aşağıdaki doğrulama kodunu kullanın.
                        </td>
                    </tr>

                    <!-- CODE BOX -->
                    <tr>
                        <td class=""code-box""
                            style=""font-size: 36px; letter-spacing: 6px; background-color: #f0f4f8; padding: 20px 0; border-radius: 8px; font-weight: bold; color: #1A2238;"">
                            {verificationCode}
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding-top: 20px; font-size: 14px; color: #888;"">
                            Bu kod 10 dakika boyunca geçerlidir.
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding-top: 30px; font-size: 12px; color: #bbb;"">
                            Eğer bu isteği siz yapmadıysanız, lütfen güvenlik ekibimizle iletişime geçin.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>",
                };

                await smtpClient.SendAsync(mailMessage);

            }
        }
        public async Task SendEmailApiAnalysisAsync(SendEmailApiAnalysisDto sendEmailApiAnalysisDto)
        {
            using (MimeMessage mailMessage = new())
            {
                mailMessage.Subject = sendEmailApiAnalysisDto.Subject;
                mailMessage.From.Add(MailboxAddress.Parse(mailInformation.Value.Username));
                mailMessage.To.Add(MailboxAddress.Parse(sendEmailApiAnalysisDto.ToMail));

                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = @$"{sendEmailApiAnalysisDto.Url} istek süresinde performans kayıbı mevcut, lütfen performan iyileştirmesi yapınız"
                };

                await smtpClient.SendAsync(mailMessage);
            }
        }

    }
}
