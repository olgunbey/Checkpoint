using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Entities.Outbox;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using System.Text.Json;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class RegisterOutboxTransaction(IdentityDbContext identityDbContext)
    {
        public async Task AddRegisterOutbox(RegisterCorporateDto registerCorporateDto, CancellationToken cancellationToken)
        {
            RegisterOutboxEvent registerOutboxEvent = new()
            {
                Mail = registerCorporateDto.Mail,
                CompanyName = registerCorporateDto.Mail.Split('@')[1].Split(".")[0]
            };
            identityDbContext.RegisterOutbox.Add(new Entities.Outbox.RegisterOutbox()
            {
                EventType = registerOutboxEvent.GetType().Name,
                ProcessedDate = null,
                Payload = JsonSerializer.Serialize(registerOutboxEvent),
            });
            await identityDbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<RegisterOutbox>> ProcessedRegister()
        {
            var notProcessedDate = identityDbContext.RegisterOutbox.Where(y => y.ProcessedDate != null);

            await notProcessedDate.ExecuteUpdateAsync(setter => setter.SetProperty(x => x.ProcessedDate, DateTime.UtcNow));


            return await notProcessedDate.ToListAsync();
        }
    }
}
