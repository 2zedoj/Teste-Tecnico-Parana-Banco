using MediatR;
using PropostaService.Domain.Abstraction;
using PropostaService.Domain.Common.Events;
using Shared.Integration.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Application.Events.Handlers
{
    public sealed class PropostaCriadaEventHandler
    : INotificationHandler<PropostaCreatedEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public PropostaCriadaEventHandler(IEventPublisher eventPublisher)
            => _eventPublisher = eventPublisher;

        public async Task Handle(
            PropostaCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            var integrationEvent = new PropostaAprovadaIntegrationEvent(
                notification.Proposta.Id,
                notification.Proposta.ClientId,
                notification.Proposta.ClienteName,
                notification.Proposta.Limite,
                notification.Proposta.MaxCartoes 
            );

            await _eventPublisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
