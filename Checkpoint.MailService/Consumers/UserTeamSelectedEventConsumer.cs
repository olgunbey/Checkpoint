using MassTransit;
using Shared.Events;

namespace Checkpoint.MailService.Consumers
{
    public class UserTeamSelectedEventConsumer : IConsumer<UserTeamSelectedEvent>
    {
        public Task Consume(ConsumeContext<UserTeamSelectedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
