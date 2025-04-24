using Checkpoint.IdentityServer.SagaOrchestration.StateMaps;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.IdentityServer.SagaOrchestration.SagaContext
{
    public class IdentityServerStateDbContext : SagaDbContext
    {
        public IdentityServerStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new IdentityServerStateMap();
            }
        }
    }
}
