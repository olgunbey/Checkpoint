using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasData(new UserTeam
            {
                Id = 1,
                CorporateId = 1,
                TeamId = 1,
            });
            builder.Property(y => y.IndividualId).IsRequired(false);
            builder.Property(y => y.CorporateId).IsRequired(false);
        }
    }
}
