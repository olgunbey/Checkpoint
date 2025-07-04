﻿
using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using MassTransit;
using Shared;
using Shared.Events;
using System.Text.Json;

namespace Checkpoint.IdentityServer.BackgroundJobs
{
    public class RegisterOutboxJob(RegisterOutboxTransaction registerOutboxTransaction, IBus bus, CompanyTransaction companyTransaction)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var registerOutboxes = await registerOutboxTransaction.ProcessedRegister();

            var registerOutboxEvents = new List<RegisterOutbox>();

            foreach (var registerOutbox in registerOutboxes)
            {
                if (registerOutbox.EventType == nameof(RegisterOutbox))
                {
                    RegisterOutbox registerOutboxEvent = JsonSerializer.Deserialize<RegisterOutbox>(registerOutbox.Payload)!;
                    registerOutboxEvent.CompanyName = companyTransaction.GetCompanyByCompanyKey(registerOutboxEvent.CompanyName).Result!.Name;
                    registerOutboxEvents.Add(registerOutboxEvent);
                }
            }
            if (registerOutboxEvents.Any())
            {
                var startedEvent = new RegisterStartEvent()
                {
                    RegisterOutboxes = registerOutboxEvents.Select(y => new RegisterOutbox()
                    {
                        Mail = y.Mail,
                        CompanyName = y.CompanyName,
                        Password = y.Password,
                        VerificationCode = y.VerificationCode,
                    }).ToList(),
                    Email = JsonSerializer.Serialize(registerOutboxEvents.Select(y => y.Mail).ToList())
                };

                var sendEndPoint = await bus.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.StateMachine}"));
                await sendEndPoint.Send(startedEvent);
            }

        }
    }
}
