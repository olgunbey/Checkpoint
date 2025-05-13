using Checkpoint.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.API.Data.Configurations
{
    public class BaseUrlConfiguration : IEntityTypeConfiguration<BaseUrl>
    {
        public void Configure(EntityTypeBuilder<BaseUrl> builder)
        {
            builder.HasData(new BaseUrl()
            {
                Id = 1,
                ProjectId = 1,
                BasePath = "https://localhost:5000/api",
            },
            new BaseUrl()
            {
                Id = 2,
                ProjectId = 2,
                BasePath = "https://localhost:5001/api",
            });
        }
    }
}
