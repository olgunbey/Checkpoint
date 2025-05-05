using Checkpoint.IdentityServer.SagaOrchestration.StateInstances;
using MassTransit;
using Shared;
using Shared.Events;

namespace Checkpoint.IdentityServer.SagaOrchestration.StateMachines
{
    public class IdentityServerStateMachine : MassTransitStateMachine<IdentityServerStateInstance>
    {


        public Event<RegisterStartEvent> RegisterStartEvent { get; set; }
        public Event<RegisterOutboxEvent> RegisterOutboxEvent { get; set; }
        public State RegisterInbox { get; set; }
        public IdentityServerStateMachine()
        {
            InstanceState(y => y.CurrentState);

            Event(() => RegisterStartEvent,
                 x => x.SelectId(context => Guid.NewGuid()));


            Event(() => RegisterOutboxEvent, stateInstance => stateInstance.CorrelateById(@event => @event.Message.CorrelationId));



            Initially(When(RegisterStartEvent)
                .Then(context =>
                {
                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.Email = context.Message.Email;
                })
                .TransitionTo(RegisterInbox)
                .Send(new Uri($"queue:{QueueConfigurations.RegisterOutboxQueue}"),
                context => new RegisterOutboxEvent(context.Saga.CorrelationId)
                {
                    RegisterOutboxes = context.Message.RegisterOutboxes,
                })
                );


            SetCompletedWhenFinalized();


        }
    }
}
