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

            var sendEndPoint = await context.GetSendEndpoint(new Uri($"{QueueConfigurations.Identity_Server_UserTeamSelected_Mail_Service}"));
            if (context.Message.IndividualId != 0)
            {
                Individual individual = (await identityDbContext.Individual.FindAsync(context.Message.IndividualId))!;

                UserTeamSelectedEvent userTeamSelectedEvent = new UserTeamSelectedEvent()
                {
                    MailAddress = individual.Mail,
                    ApiUrl = context.Message.ApiUrl,
                };
                await sendEndPoint.Send(userTeamSelectedEvent);

            }
            if (context.Message.TeamId != 0)
            {
                var userTeam = await identityDbContext.UserTeam
                    .Include(y => y.Individual)
                    .Include(y => y.Corporate)
                    .Where(y => y.TeamId == context.Message.TeamId)
                    .ToListAsync();

                foreach (var team in userTeam)
                {
                    string mailAddress = string.Empty;
                    if (team.IndividualId != 0)
                    {
                        mailAddress = team.Individual.Mail;
                    }
                    if (team.IndividualId != 0)
                    {
                        mailAddress = team.Individual.Mail;
                    }

                    UserTeamSelectedEvent userTeamSelectedEvent = new UserTeamSelectedEvent()
                    {
                        MailAddress = mailAddress,
                    };
                    await sendEndPoint.Send(userTeamSelectedEvent);
                }
            }

        }
    }
}
