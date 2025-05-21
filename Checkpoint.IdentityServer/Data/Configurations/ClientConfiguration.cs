using Checkpoint.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasData(new Client()
            {
                Id = 1,
                ClientId = "checkpoint-client",
                GrantType = "resourceowner",
                Issuer = "https://localhost:7253",
                Audience = "https://localhost:5000",
                ClientSecret = "checkpointsecret"
            });
        }
    }
}
