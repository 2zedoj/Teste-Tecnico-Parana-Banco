using MassTransit;
using MediatR;
using Shared.Integration.IntegrationEvents;


namespace PropostaService.Infrastructure.Messaging
{
    public sealed class GerarPropostaConsumer
    : IConsumer<ClientCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public GerarPropostaConsumer(IMediator mediator)
            => _mediator = mediator;

        public async Task Consume(
            ConsumeContext<ClientCreatedIntegrationEvent> context)
        {
            await _mediator.Publish(
                context.Message,
                context.CancellationToken);
        }
    }
}
