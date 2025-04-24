using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Shared.Events;

namespace Checkpoint.IdentityServer.Consumers
{
    public class MailSentEventConsumer(IIdentityDbContext identityDbContext, CompanyTransaction companyTransaction) : IConsumer<MailSentEvent>
    {
        public async Task Consume(ConsumeContext<MailSentEvent> context)
        {
            identityDbContext.Corporate.Add(new Entities.Corporate()
            {
                Mail = context.Message.Email,
                Password = context.Message.Password,
                VerificationCode = Verification.GenerateVerification(),
                CompanyId = companyTransaction.GetCompanyByCompanyName(context.Message.CompanyName).Result!.Id,
            });
            await identityDbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
