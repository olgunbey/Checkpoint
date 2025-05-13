using Checkpoint.MailService.Interfaces;
using MassTransit;
using Shared.Events;

namespace Checkpoint.MailService.Consumers
{
    public class UserTeamSelectedEventConsumer(IMailService mailService, IMailDbContext mailDbContext) : IConsumer<UserTeamSelectedEvent>
    {
        public async Task Consume(ConsumeContext<UserTeamSelectedEvent> context)
        {
            try
            {
                await mailService.SendEmailApiAnalysisAsync(new Dtos.SendEmailApiAnalysisDto()
                {
                    Url = context.Message.ApiUrl,
                    ToMail = context.Message.MailAddress
                });
            }
            catch (Exception)
            {
                mailDbContext.NotSentMail.Add(new Entities.NotSentMail()
                {
                    Email = context.Message.MailAddress,
                });
                await mailDbContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}
