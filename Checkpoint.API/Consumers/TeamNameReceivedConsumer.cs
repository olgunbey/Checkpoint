using Checkpoint.API.Dtos;
using Checkpoint.API.Interfaces;
using MassTransit;
using Shared.Events;

namespace Checkpoint.API.Consumers
{
    public class TeamNameReceivedConsumer(IApplicationDbContext applicationDbContext) : IConsumer<TeamNameReceivedEvent>
    {
        public async Task Consume(ConsumeContext<TeamNameReceivedEvent> context)
        {
            var teamIds = context.Message.Teams.Select(y => y.TeamId);
            var projects = applicationDbContext.Project
                .Where(y => y.TeamId.HasValue)
                .IntersectBy(teamIds, project => project.TeamId!.Value);

            var response = projects.Select(y => new GetAllProjectAndTeamResponseDto()
            {
                TeamId = y.TeamId,
                ProjectName = y.ProjectName,
                TeamName = context.Message.Teams.Single(y => y.TeamId == y.TeamId).TeamName
            }).ToList();

            await context.RespondAsync(Shared.Common.ResponseDto<List<GetAllProjectAndTeamResponseDto>>.Success(response, 200));
        }
    }
}
