using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(new Permission
            {
                Id = 1,
                Name = "Admin"

            }, new Permission
            {
                Id = 2,
                Name = "Okuma"
            }, new Permission
            {
                Id = 3,
                Name = "Yazma"
            });
        }
    }
}
