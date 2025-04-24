using MassTransit;

namespace Checkpoint.IdentityServer.SagaOrchestration.StateInstances
{
    public class IdentityServerStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string Email { get; set; } = "DefaultName";
        public DateTime CreatedDate { get; set; }
    }
}
