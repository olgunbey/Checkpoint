using Checkpoint.API.Interfaces;
using MassTransit;
using Shared.Common;
using Shared.Dtos;
using Shared.Events;

namespace Checkpoint.API.Consumers
{
    public class TeamNameReceivedConsumer(IApplicationDbContext applicationDbContext) : IConsumer<TeamNameReceivedEvent>
    {
        public async Task Consume(ConsumeContext<TeamNameReceivedEvent> context)
        {
            var teamIds = context.Message.Teams.Select(t => t.TeamId);

            var projects = applicationDbContext.Project
                .Where(p => p.TeamId != null && teamIds.Contains(p.TeamId.Value))
                .Select(p => new
                {
                    p.TeamId,
                    Project = new ProjectDto
                    {
                        ProjectId = p.Id,
                        ProjectName = p.ProjectName
                    }
                })
                .AsEnumerable();


            var response = context.Message.Teams.Select(t => new GetAllProjectAndTeamResponseDto
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
                ProjectDto = projects
                    .Where(p => p.TeamId == t.TeamId)
                    .Select(p => p.Project)
                    .ToList()
            }).ToList();
            await context.RespondAsync(ResponseDto<List<GetAllProjectAndTeamResponseDto>>.Success(response, 200));
        }
    }
}
