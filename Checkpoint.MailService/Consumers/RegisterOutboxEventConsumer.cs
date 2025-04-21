using Checkpoint.MailService.Data.DatabaseTransactions;
using MassTransit;
using Shared.Events;
using System.Data;
using System.Text;

namespace Checkpoint.MailService.Consumers
{
    public class RegisterOutboxEventConsumer(MailInboxTransaction mailInboxTransaction, IPublishEndpoint publishEndpoint) : IConsumer<RegisterOutboxEventBatch>
    {
        public async Task Consume(ConsumeContext<RegisterOutboxEventBatch> context)
        {
            await mailInboxTransaction.AddMailInboxAsync(context.Message.Events, CancellationToken.None);

            var getAllRegisterOutbox = await mailInboxTransaction.GetAllMailInbox();

            var queryAbleRegisterOutbox = getAllRegisterOutbox.Where(y => !y.Processed).ToList();

            Random rnd = new Random();
            foreach (var item in queryAbleRegisterOutbox)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 1; i <= 10; i++)
                {
                    int ascii = rnd.Next(48, 91);
                    char ch = (char)ascii;
                    stringBuilder.Append(ch);
                }

                MailSentEvent mailSentEvent = new()
                {
                    Email = item.Mail,
                    CompanyName = item.CorporateName,
                    Password = stringBuilder.ToString()
                };
                item.Processed = true;
                await mailInboxTransaction.SaveChangesAsync(CancellationToken.None);

                //bu kısım mail atma kısmı. Burada password'u kullanıcıya mail yoluyla döneceğiz.
                await publishEndpoint.Publish(mailSentEvent);
            }
        }
    }
}
