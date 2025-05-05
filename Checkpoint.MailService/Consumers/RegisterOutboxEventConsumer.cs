using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using MassTransit;
using Shared.Events;
using System.Data;

namespace Checkpoint.MailService.Consumers
{
    public class RegisterOutboxEventConsumer(IMailDbContext mailDbContext, MailServices.MailService mailService) : IConsumer<RegisterOutboxEvent>
    {
        public async Task Consume(ConsumeContext<RegisterOutboxEvent> context)
        {
            var registerCorporateMails = context.Message.RegisterOutboxes.Select(y => new RegisterInbox()
            {
                Mail = y.Mail,
                CorporateName = y.CompanyName,
                Password = y.Password,
                Processed = false,
                VerificationCode = y.VerificationCode
            });

            mailDbContext.RegisterInbox.AddRange(registerCorporateMails);
            await mailDbContext.SaveChangesAsync(CancellationToken.None);

            var getAllRegisterInbox = mailDbContext.RegisterInbox.Where(y => !y.Processed).ToList();

            foreach (var registerInbox in getAllRegisterInbox)
            {
                registerInbox.Processed = true;
                await mailDbContext.SaveChangesAsync(CancellationToken.None);

                try
                {
                    await mailService.SendEmail(registerInbox.Mail, "Verification", registerInbox.VerificationCode, Enums.SendEmailType.Verification);

                }
                catch (Exception)
                {
                    mailDbContext.NotSentMail.Add(new NotSentMail()
                    {
                        Email = registerInbox.Mail,
                        VerificationCode = registerInbox.VerificationCode,
                    });
                    await mailDbContext.SaveChangesAsync(CancellationToken.None);
                    continue;
                }
            }
        }
    }
}
