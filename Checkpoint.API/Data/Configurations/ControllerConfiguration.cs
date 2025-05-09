using Checkpoint.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.API.Data.Configurations
{
    public class ControllerConfiguration : IEntityTypeConfiguration<Checkpoint.API.Entities.Controller>
    {
        public void Configure(EntityTypeBuilder<Controller> builder)
        {
            builder.HasData(new Controller()
            {
                Id = 1,
                BaseUrlId = 1,
                CreateUserId = 1,
                UpdateUserId = 1,
                ControllerPath = "User"
            },
            new Controller()
            {
                Id = 2,
                BaseUrlId = 1,
                CreateUserId = 1,
                UpdateUserId = 1,
                ControllerPath = "Teacher"
            });
        }
    }
}
