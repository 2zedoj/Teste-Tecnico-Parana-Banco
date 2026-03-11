using MassTransit;
using PropostaService.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Infrastructure.Messaging
{
    public sealed class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
            => _publishEndpoint = publishEndpoint;

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : class
            => await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
