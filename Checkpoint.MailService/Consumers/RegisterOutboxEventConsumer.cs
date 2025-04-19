using Checkpoint.MailService.Data.DatabaseTransactions;
using MassTransit;
using Shared.Events;

namespace Checkpoint.MailService.Consumers
{
    public class RegisterOutboxEventConsumer(MailInboxTransaction mailInboxTransaction) : IConsumer<RegisterOutboxEventBatch>
    {
        public async Task Consume(ConsumeContext<RegisterOutboxEventBatch> context)
        {
            await mailInboxTransaction.AddMailInboxAsync(context.Message.Events, CancellationToken.None);

            var getAllRegisterOutbox = await mailInboxTransaction.GetAllMailInbox();


        }
    }
}
