using Checkpoint.IdentityServer.SagaOrchestration.StateInstances;
using MassTransit;
using Shared;
using Shared.Events;

namespace Checkpoint.IdentityServer.SagaOrchestration.StateMachines
{
    public class IdentityServerStateMachine : MassTransitStateMachine<IdentityServerStateInstance>
    {

        private readonly ILogger<IdentityServerStateMachine> _logger;

        public Event<RegisterStartEvent> RegisterStartEvent { get; set; }
        public Event<RegisterOutboxEventBatch> RegisterOutboxEventBatchEvent { get; set; }
        public Event<MailSentEvent> MailSentEvent { get; set; }
        public State RegisterOutboxEventBatchState { get; set; }
        public State MailSentEventState { get; set; }
        public IdentityServerStateMachine(ILogger<IdentityServerStateMachine> logger)
        {
            _logger = logger;
            InstanceState(y => y.CurrentState);

            Event(() => RegisterStartEvent,
                 x => x.SelectId(context => Guid.NewGuid()));


            Event(() => RegisterOutboxEventBatchEvent, stateInstance => stateInstance.CorrelateById(@event => @event.Message.CorrelationId));


            Event(() => MailSentEvent, stateInstance => stateInstance.CorrelateById(@event => @event.Message.CorrelationId));


            Initially(When(RegisterStartEvent)
                .Then(context =>
                {
                    context.Saga.CreatedDate = DateTime.UtcNow;

                    _logger.LogInformation("Event gönderildi correlationId:", context.Saga.CorrelationId);

                })
                .TransitionTo(RegisterOutboxEventBatchState)
                .Send(new Uri($"queue:{QueueConfigurations.RegisterOutboxQueue}"),
                context => new RegisterOutboxEventBatch(context.Saga.CorrelationId)
                {
                    RegisterOutboxes = context.Message.RegisterOutboxes

                })
                );

            During(RegisterOutboxEventBatchState,
                When(MailSentEvent)
                .TransitionTo(MailSentEventState)
                .Then(context =>
                {
                    context.Saga.Email = context.Message.Email;
                })
                .Send(new Uri($"queue:{QueueConfigurations.MailSentEvent}"),
                context => new MailSentEvent(context.Saga.CorrelationId)
                {
                    Email = context.Message.Email,
                    Password = context.Message.Password,
                    CompanyName = context.Message.CompanyName,
                }));

            SetCompletedWhenFinalized();


        }
    }
}
