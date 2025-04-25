using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Hash;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class RegisterOutboxTransaction(IdentityDbContext identityDbContext)
    {
        public async Task AddRegisterAsync(RegisterCorporateDto registerCorporateDto, CancellationToken cancellationToken)
        {
            string companyName = registerCorporateDto.Mail.Split('@')[1].Split(".")[0];

            string hashingPassword = Hashing.HashPassword(registerCorporateDto.Password);
            string verificationCode = Verification.GenerateVerification();

            Shared.Events.RegisterOutbox registerOutboxEvent = new()
            {
                Mail = registerCorporateDto.Mail,
                CompanyName = companyName,
                Password = hashingPassword,
                VerificationCode = verificationCode
            };
            identityDbContext.RegisterOutbox.Add(new Entities.Outbox.RegisterOutbox()
            {
                EventType = registerOutboxEvent.GetType().Name,
                ProcessedDate = null,
                Payload = JsonSerializer.Serialize(registerOutboxEvent),
            });
            var hasCompany = await identityDbContext.Company.FirstOrDefaultAsync(y => y.Key == companyName);

            if (hasCompany == null)
            {
                throw new Exception("Not found Company!!");
            }
            identityDbContext.Corporate.Add(new Entities.Corporate()
            {
                Mail = registerCorporateDto.Mail,
                Password = hashingPassword,
                VerificationCode = verificationCode,
                CompanyId = hasCompany.Id
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
