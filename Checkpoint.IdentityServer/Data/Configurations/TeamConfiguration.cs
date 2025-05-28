using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(new Team
            {
                Id = 1,
                CompanyId = 1,
                Name = "Sigorta"
            },
            new Team
            {
                Id = 2,
                CompanyId = 1,
                Name = "Araç kiralama"
            });
        }
    }
}
