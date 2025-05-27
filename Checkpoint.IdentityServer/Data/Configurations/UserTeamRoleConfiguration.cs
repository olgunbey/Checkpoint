using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class UserTeamRoleConfiguration : IEntityTypeConfiguration<UserTeamRole>
    {
        public void Configure(EntityTypeBuilder<UserTeamRole> builder)
        {
            builder.HasData(new UserTeamRole
            {
                Id = 1,
                RoleId = 1,
                UserTeamId = 1
            });
        }
    }
}
