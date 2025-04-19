using Checkpoint.IdentityServer.Entities.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class RegisterOutboxConfiguration : IEntityTypeConfiguration<RegisterOutbox>
    {
        public void Configure(EntityTypeBuilder<RegisterOutbox> builder)
        {
            builder.Property(y => y.ProcessedDate).IsRequired(false);
        }
    }
}
