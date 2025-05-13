using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.API.Data.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<Entities.Action>
    {
        public void Configure(EntityTypeBuilder<Entities.Action> builder)
        {
            builder.Property(y => y.Query)
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(y => y.Body)
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(y => y.Header)
                .HasColumnType("jsonb")
                .IsRequired(false);
        }
    }
}
