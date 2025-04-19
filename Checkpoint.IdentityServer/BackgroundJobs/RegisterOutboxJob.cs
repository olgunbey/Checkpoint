
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using MassTransit;
using Shared.Events;
using System.Text.Json;

namespace Checkpoint.IdentityServer.BackgroundJobs
{
    public class RegisterOutboxJob(RegisterOutboxTransaction registerOutboxTransaction, IPublishEndpoint publishEndpoint, CompanyTransaction companyTransaction)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var registerOutboxes = await registerOutboxTransaction.ProcessedRegister();

            var registerOutboxEvents = new List<RegisterOutboxEvent>();

            foreach (var registerOutbox in registerOutboxes)
            {
                if (registerOutbox.EventType == nameof(RegisterOutboxEvent))
                {
                    RegisterOutboxEvent registerOutboxEvent = JsonSerializer.Deserialize<RegisterOutboxEvent>(registerOutbox.Payload)!;
                    registerOutboxEvent.CompanyName = companyTransaction.GetCompanyByCompanyKey(registerOutboxEvent.CompanyName).Result!.Name;
                    registerOutboxEvents.Add(registerOutboxEvent);
                }
            }
            if (registerOutboxEvents.Any())
            {
                var batchEvent = new RegisterOutboxEventBatch
                {
                    Events = registerOutboxEvents
                };

                await publishEndpoint.Publish(batchEvent);
            }

        }
    }
}
