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

            //builder.HasData(new Entities.Action()
            //{
            //    Id = 1,
            //    ControllerId = 1,
            //    RequestType = Enums.RequestType.Post,
            //    CreatedDate = DateTime.Now,
            //    UpdatedDate = DateTime.Now,
            //    CreateUserId = 1,
            //    UpdateUserId = 1,
            //    ActionPath = "AddUser",
            //    Body = new List<RequestPayloads.Body>()
            //    {
            //        new RequestPayloads.Body()
            //        {
            //            Key="Name",
            //            Value="olgun"
            //        },
            //        new RequestPayloads.Body()
            //        {
            //            Key="Age",
            //            Value=12
            //        }
            //    },
            //},
            //new Entities.Action()
            //{
            //    Id = 2,
            //    ControllerId = 1,
            //    RequestType = Enums.RequestType.Get,
            //    CreatedDate = DateTime.Now,
            //    UpdatedDate = DateTime.Now,
            //    CreateUserId = 1,
            //    UpdateUserId = 1,
            //    ActionPath = "getAllUser",
            //    Header = new List<RequestPayloads.Header>()
            //    {
            //        new RequestPayloads.Header()
            //        {
            //            Key="userId",
            //            Value=1
            //        }
            //    }
            //});
        }
    }
}
