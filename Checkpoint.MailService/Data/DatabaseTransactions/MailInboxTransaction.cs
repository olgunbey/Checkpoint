using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using Shared.Events;

namespace Checkpoint.MailService.Data.DatabaseTransactions
{
    public class MailInboxTransaction(IMailDbContext mailInboxDbContext)
    {
        public async Task AddMailInboxAsync(List<RegisterOutboxEvent> registerOutboxEvents, CancellationToken cancellationToken)
        {
            var registerCorporateMails = registerOutboxEvents.Select(y => new RegisterInbox()
            {
                Mail = y.Mail,
                CorporateName = y.CompanyName,
                Processed = false
            });

            mailInboxDbContext.RegisterInbox.AddRange(registerCorporateMails);
            await mailInboxDbContext.SaveChangesAsync(cancellationToken);
        }
        public Task<IQueryable<RegisterInbox>> GetAllMailInbox()
        {
            return Task.FromResult(mailInboxDbContext.RegisterInbox.Where(y => !y.Processed));
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await mailInboxDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
