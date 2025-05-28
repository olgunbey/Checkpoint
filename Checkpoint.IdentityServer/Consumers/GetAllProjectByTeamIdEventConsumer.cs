using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;

namespace Checkpoint.IdentityServer.Consumers
{
    public class GetAllProjectByTeamIdEventConsumer(IIdentityDbContext identityDbContext) : IConsumer<GetAllProjectByTeamIdEvent>
    {
        public async Task Consume(ConsumeContext<GetAllProjectByTeamIdEvent> context)
        {
            var listTeam = identityDbContext.Team.IntersectBy(context.Message.TeamId, keySelector => keySelector.Id);
            var sendEndpoint = await context.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.Identity_GetTeamName_Checkpoint_Api}"));

            var sendEventTeams = await listTeam.Select(y => new Shared.Events.Team
            {
                TeamId = y.Id,
                TeamName = y.Name,
            }).ToListAsync();
            await sendEndpoint.Send(new TeamNameReceivedEvent { Teams = sendEventTeams });
        }
    }
}
