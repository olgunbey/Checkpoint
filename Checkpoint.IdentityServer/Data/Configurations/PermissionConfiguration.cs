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
            }
            , new Permission
            {
                Id = 2,
                Name = "Ekleme"
            }
            , new Permission
            {
                Id = 3,
                Name = "Güncelleme"
            }
            , new Permission
            {
                Id = 4,
                Name = "Onay"
            }
            , new Permission
            {
                Id = 5,
                Name = "Rol Atama"
            }
            , new Permission
            {
                Id = 6,
                Name = "Rol Güncelleme"
            }
            , new Permission
            {
                Id = 7,
                Name = "Rol Silme"
            }
            , new Permission
            {
                Id = 8,
                Name = "Rol Ekleme"
            }
            , new Permission
            {
                Id = 9,
                Name = "Okuma"
            });
        }
    }
}
