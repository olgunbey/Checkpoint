using Checkpoint.IdentityServer.SagaOrchestration.StateInstances;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkpoint.IdentityServer.SagaOrchestration.StateMaps
{
    public class IdentityServerStateMap : SagaClassMap<IdentityServerStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<IdentityServerStateInstance> entity, ModelBuilder model)
        {

        }
    }
}
