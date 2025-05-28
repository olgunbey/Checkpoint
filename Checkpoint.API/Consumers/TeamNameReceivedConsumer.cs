using Checkpoint.API.Dtos;
using Checkpoint.API.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Checkpoint.API.Consumers
{
    public class TeamNameReceivedConsumer(IApplicationDbContext applicationDbContext) : IConsumer<TeamNameReceivedEvent>
    {
        public async Task Consume(ConsumeContext<TeamNameReceivedEvent> context)
        {
            var projects = await applicationDbContext.Project.Where(y => y.TeamId == context.Message.TeamId).ToListAsync();

            var response = projects.Select(y => new GetAllProjectAndTeamResponseDto()
            {
                TeamId = y.TeamId,
                ProjectName = y.ProjectName,
                TeamName = context.Message.TeamName
            }).ToList();

            await context.RespondAsync(Shared.Common.ResponseDto<List<GetAllProjectAndTeamResponseDto>>.Success(response, 200));
        }
    }
}
