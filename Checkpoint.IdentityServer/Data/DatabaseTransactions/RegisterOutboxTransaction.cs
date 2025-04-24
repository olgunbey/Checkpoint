using Checkpoint.IdentityServer.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class RegisterOutboxTransaction(IdentityDbContext identityDbContext)
    {
        public async Task AddRegisterAsync(RegisterCorporateDto registerCorporateDto, CancellationToken cancellationToken)
        {
            Shared.Events.RegisterOutbox registerOutboxEvent = new()
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
        public async Task<List<Entities.Outbox.RegisterOutbox>> ProcessedRegister()
        {
            var notProcessedDate = identityDbContext.RegisterOutbox.Where(y => y.ProcessedDate == null);
            var data = await notProcessedDate.ToListAsync();
            await notProcessedDate.ExecuteUpdateAsync(setter => setter.SetProperty(x => x.ProcessedDate, DateTime.UtcNow));
            return data;
        }
    }
}
