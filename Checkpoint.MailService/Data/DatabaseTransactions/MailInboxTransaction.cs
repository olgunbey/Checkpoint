using Checkpoint.MailService.Entities;
using Checkpoint.MailService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Checkpoint.MailService.Data.DatabaseTransactions
{
    public class MailInboxTransaction(IMailInboxDbContext mailInboxDbContext)
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
        public async Task<List<RegisterInbox>> GetAllMailInbox()
        {
            return await mailInboxDbContext.RegisterInbox.Where(y => !y.Processed).ToListAsync();
        }
    }
}
