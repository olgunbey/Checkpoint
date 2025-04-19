
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Entities;
using MassTransit;
using Shared.Events;
using System.Text.Json;

namespace Checkpoint.IdentityServer.BackgroundJobs
{
    public class RegisterOutboxJob(RegisterOutboxTransaction registerOutboxTransaction, IPublishEndpoint publishEndpoint, CorporateTransaction corporateTransaction, CompanyTransaction companyTransaction) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var registerOutboxes = await registerOutboxTransaction.ProcessedRegister();

            var registerOutboxEvents = new List<RegisterOutboxEvent>();

            foreach (var registerOutbox in registerOutboxes)
            {
                if (registerOutbox.EventType == nameof(RegisterOutboxEvent))
                {
                    RegisterOutboxEvent registerOutboxEvent = JsonSerializer.Deserialize<RegisterOutboxEvent>(registerOutbox.Payload)!;
                    registerOutboxEvents.Add(registerOutboxEvent);
                }
            }
            if (registerOutboxEvents.Any())
            {
                var batchEvent = new RegisterOutboxEventBatch
                {
                    Events = registerOutboxEvents
                };

                var corporates = registerOutboxEvents.Select(y => new Corporate()
                {
                    CompanyId = companyTransaction.GetCompanyByCompanyName(y.CompanyName).Result.Id,
                    Mail = y.Mail,
                    Password = "random123"
                });
                await corporateTransaction.CorporateAddRange(corporates, stoppingToken);


                await publishEndpoint.Publish(batchEvent);
            }

        }
    }
}
