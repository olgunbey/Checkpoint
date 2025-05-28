using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class CorporateConfiguration : IEntityTypeConfiguration<Corporate>
    {
        public void Configure(EntityTypeBuilder<Corporate> builder)
        {
            builder.HasData(new Corporate
            {
                Id = 1,
                Password = "acf3d886b0df7919742e5191fd5a0a745e887e6e1e3e75985394dd217c83979589543bde8e3037e69bff84f884fc55d1cd846cb3a7dbf04b25ca104ed93d7c5b",
                CompanyId = 1,
                Mail = "olgunsahin0161@hotmail.com",
                VerificationCode = "A123-B213"
            });
            builder.Property(y => y.InvitationId).IsRequired(false);
            builder.Property(y => y.Id).UseHiLo();
        }
    }
}
