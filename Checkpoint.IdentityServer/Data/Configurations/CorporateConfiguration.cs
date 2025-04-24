using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class CorporateConfiguration : IEntityTypeConfiguration<Corporate>
    {
        public void Configure(EntityTypeBuilder<Corporate> builder)
        {
            builder.Property(y => y.InvitationId).IsRequired(false);
        }
    }
}
