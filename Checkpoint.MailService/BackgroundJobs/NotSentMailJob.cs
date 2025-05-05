using Checkpoint.MailService.Interfaces;

namespace Checkpoint.MailService.BackgroundJobs
{
    public class NotSentMailJob(IMailDbContext mailDbContext, MailServices.MailService mailService)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var NotProcesseds = mailDbContext.NotSentMail.Where(y => !y.Processed).ToList();

            foreach (var notProcessed in NotProcesseds)
            {
                try
                {
                    await mailService.SendEmail(notProcessed.Email, "Verification", notProcessed.VerificationCode, Enums.SendEmailType.Verification);
                    notProcessed.Processed = true;
                    await mailDbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    continue;
                }

            }
        }
    }
}
