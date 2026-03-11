using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Integration.IntegrationEvents
{
    public record ClientCreatedIntegrationEvent(
        Guid ClientId,
        string Name,
        string Document,
        string Email,
        double Renda,
        int Score
    ) : INotification;
}
