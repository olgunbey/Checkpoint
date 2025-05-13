using Checkpoint.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.API.Data.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(y => y.IndividualId).IsRequired(false);
            builder.Property(y => y.TeamId).IsRequired(false);
            builder.HasData(new Project()
            {
                Id = 1,
                ProjectName = "Job Projesi",
                TeamId = 1,

            },
            new Project()
            {
                Id = 2,
                ProjectName = "Otoyol Projesi",
                TeamId = 1,

            });
        }
    }
}
