using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Dtos;
using Shared.Events;



namespace Checkpoint.IdentityServer.Consumers
{
    public class GetAllProjectByTeamIdEventConsumer(IIdentityDbContext identityDbContext, IRequestClient<TeamNameReceivedEvent> requestClient) : IConsumer<GetAllProjectByTeamIdEvent>
    {
        public async Task Consume(ConsumeContext<GetAllProjectByTeamIdEvent> context)
        {
            var listTeam = identityDbContext.UserTeam
                .Include(y => y.Team)
                .Where(y => y.CorporateId == context.Message.UserId)
                .AsEnumerable()
                .IntersectBy(context.Message.TeamId, keySelector => keySelector.TeamId).ToList();


            var sendEventTeams = listTeam.Select(y => new TeamEvent
            {
                TeamId = y.Id,
                TeamName = y.Team.Name,
            }).ToList();

            var response = await requestClient.GetResponse<ResponseDto<List<GetAllProjectAndTeamResponseDto>>>(new TeamNameReceivedEvent { Teams = sendEventTeams });

            await context.RespondAsync(response.Message);
        }
    }
}
