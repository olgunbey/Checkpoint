using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using MassTransit;
using Shared.Events;
using System.Data;

namespace Checkpoint.MailService.Consumers
{
    public class RegisterOutboxEventConsumer(IBus bus, IMailDbContext mailDbContext) : IConsumer<RegisterOutboxEvent>
    {
        public async Task Consume(ConsumeContext<RegisterOutboxEvent> context)
        {
            var registerCorporateMails = context.Message.RegisterOutboxes.Select(y => new RegisterInbox()
            {
                Mail = y.Mail,
                CorporateName = y.CompanyName,
                Password = y.Password,
                Processed = false
            });

            mailDbContext.RegisterInbox.AddRange(registerCorporateMails);
            await mailDbContext.SaveChangesAsync(CancellationToken.None);

            var getAllRegisterInbox = mailDbContext.RegisterInbox.Where(y => !y.Processed).ToList();

            foreach (var registerInbox in getAllRegisterInbox)
            {
                try
                {
                    registerInbox.Processed = true;
                    await mailDbContext.SaveChangesAsync(CancellationToken.None);

                    //bu kısım mail atma kısmı. Burada password'u kullanıcıya mail yoluyla döneceğiz.

                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
