using Checkpoint.MailService.Exceptions;
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
                throw new NotSendEmailApiException("api analizi için mail gönderilemedi!!");
            }
        }
    }
}
