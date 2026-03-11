using MassTransit;
using MediatR;
using Shared.Integration.IntegrationEvents;

namespace CartaoService.Infrastructure.Messaging
{
    public sealed class EmitirCartoesConsumer
    : IConsumer<PropostaAprovadaIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public EmitirCartoesConsumer(IMediator mediator)
            => _mediator = mediator;

        public async Task Consume(
            ConsumeContext<PropostaAprovadaIntegrationEvent> context)
        {
            await _mediator.Publish(
                context.Message,
                context.CancellationToken);
        }
    }
}
