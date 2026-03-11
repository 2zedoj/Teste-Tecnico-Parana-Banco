using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Common.Events;
using MediatR;
using Shared.Integration.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Events.Handlers
{
    public sealed class ClientCreatedEventHandler
    : INotificationHandler<CreateclientEventhandler>  // ← MediatR
    {
        private readonly IEventPublisher _eventPublisher;

        public ClientCreatedEventHandler(IEventPublisher eventPublisher)
            => _eventPublisher = eventPublisher;

        public async Task Handle(
            CreateclientEventhandler notification,
            CancellationToken cancellationToken)
        {
            var integrationEvent = new ClientCreatedIntegrationEvent(
                notification.Client.Id,
                notification.Client.Name,
                notification.Client.Document.Value,
                notification.Client.Email,
                notification.Client.Renda,
                notification.Client.Score
            );

            await _eventPublisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
