using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class UserTeamPermissionConfiguration : IEntityTypeConfiguration<UserTeamPermission>
    {
        public void Configure(EntityTypeBuilder<UserTeamPermission> builder)
        {
            builder.HasData(new UserTeamPermission
            {
                Id = 1,
                PermissionId = 1,
                UserTeamId = 1,
            }, new UserTeamPermission
            {
                Id = 2,
                PermissionId = 2,
                UserTeamId = 1
            });
        }
    }
}
