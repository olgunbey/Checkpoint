using Checkpoint.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.API.Data.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.Property(y => y.CorporateId).IsRequired(false);
            builder.Property(y => y.IndividualId).IsRequired(false);
        }
    }
}
