using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Shared;
using Shared.Common;
using Shared.Dtos;
using Shared.Events;

namespace Checkpoint.IdentityServer.Consumers
{
    public class GetAllProjectByTeamIdEventConsumer(IIdentityDbContext identityDbContext, IRequestClient<TeamNameReceivedEvent> requestClient) : IConsumer<GetAllProjectByTeamIdEvent>
    {
        public async Task Consume(ConsumeContext<GetAllProjectByTeamIdEvent> context)
        {
            var listTeam = identityDbContext.Team.AsEnumerable().IntersectBy(context.Message.TeamId, keySelector => keySelector.Id);

            var sendEventTeams = listTeam.Select(y => new Shared.Events.Team
            {
                TeamId = y.Id,
                TeamName = y.Name,
            }).ToList();

            var response = await requestClient.GetResponse<ResponseDto<List<GetAllProjectAndTeamResponseDto>>>(new TeamNameReceivedEvent { Teams = sendEventTeams });

            await context.RespondAsync(response.Message);
        }
    }
}
