using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Hash;
using System.Text.Json;

namespace Checkpoint.IdentityServer.Data.DatabaseTransactions
{
    public class RegisterOutboxTransaction(IIdentityDbContext identityDbContext)
    {
        public async Task<ResponseDto<RegisterResponseDto>> AddRegisterAsync(RegisterCorporateDto registerCorporateDto, CancellationToken cancellationToken)
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
            var addedCorporate = new Entities.Corporate()
            {
                Mail = registerCorporateDto.Mail,
                Password = hashingPassword,
                VerificationCode = verificationCode,
                CompanyId = hasCompany.Id
            };
            await identityDbContext.Corporate.AddAsync(addedCorporate);
            await identityDbContext.SaveChangesAsync(cancellationToken);
            var response = new RegisterResponseDto()
            {
                Id = addedCorporate.Id,
            };
            return ResponseDto<RegisterResponseDto>.Success(response, 200);

        }
        public async Task<List<Entities.Outbox.RegisterOutbox>> ProcessedRegister()
        {
            var notProcessedDate = identityDbContext.RegisterOutbox.Where(y => y.ProcessedDate == null);
            var data = await notProcessedDate.ToListAsync();
            await notProcessedDate.ExecuteUpdateAsync(setter => setter.SetProperty(x => x.ProcessedDate, DateTime.UtcNow));
            return data;
        }
        public async Task<ResponseDto<NoContent>> CorporateVerification(int id, string verificationCode)
        {
            bool hasCorporate = await identityDbContext.Corporate.AnyAsync(y => y.Id == id && y.VerificationCode == verificationCode);

            if (!hasCorporate)
            {
                return ResponseDto<NoContent>.Fail("Not found corporate", 404);
            }
            return ResponseDto<NoContent>.Success(204);


        }
    }
}
