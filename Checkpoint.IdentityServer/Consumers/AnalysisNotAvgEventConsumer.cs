using Checkpoint.IdentityServer.Entities;
using Checkpoint.IdentityServer.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;

namespace Checkpoint.IdentityServer.Consumers
{
    public class AnalysisNotAvgEventConsumer(IIdentityDbContext identityDbContext) : IConsumer<AnalysisNotAvgEvent>
    {
        public async Task Consume(ConsumeContext<AnalysisNotAvgEvent> context)
        {

            var sendEndPoint = await context.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.Identity_Server_UserTeamSelected_Mail_Service}"));
            UserTeamSelectedEvent userTeamSelectedEvent = new();
            if (context.Message.IndividualId != null)
            {
                Individual individual = (await identityDbContext.Individual.FindAsync(context.Message.IndividualId))!;

                userTeamSelectedEvent = new UserTeamSelectedEvent()
                {
                    MailAddress = individual!.Mail,
                    ApiUrl = context.Message.ApiUrl,
                };
                await sendEndPoint.Send(userTeamSelectedEvent);

            }
            if (context.Message.TeamId != 0)
            {
                var userTeam = await identityDbContext.UserTeam
                    .Where(y => y.TeamId == context.Message.TeamId)
                    .Include(y => y.Corporate)
                    .ToListAsync();

                foreach (var team in userTeam)
                {
                    string mailAddress = string.Empty;

                    userTeamSelectedEvent = new UserTeamSelectedEvent()
                    {
                        MailAddress = team.Corporate.Mail,
                        ApiUrl = context.Message.ApiUrl
                    };
                    await sendEndPoint.Send(userTeamSelectedEvent);
                }
            }

        }
    }
}
