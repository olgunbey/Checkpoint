using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Shared;
using Shared.Events;

namespace Checkpoint.IdentityServer.Consumers
{
    public class GetAllProjectByTeamIdEventConsumer(IIdentityDbContext identityDbContext) : IConsumer<GetAllProjectByTeamIdEvent>
    {
        public async Task Consume(ConsumeContext<GetAllProjectByTeamIdEvent> context)
        {
            var team = (await identityDbContext.Team.FindAsync(context.Message.TeamId))!;
            var sendEndpoint = await context.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.Identity_GetTeamName_Checkpoint_Api}"));
            await sendEndpoint.Send(new TeamNameReceivedEvent { TeamName = team.Name, TeamId = context.Message.TeamId });
        }
    }
}
