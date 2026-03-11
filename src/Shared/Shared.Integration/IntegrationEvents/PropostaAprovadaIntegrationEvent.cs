using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Integration.IntegrationEvents
{
    public record PropostaAprovadaIntegrationEvent(
        Guid PropostaId,
        Guid ClienteId,
        string ClienteNome,
        decimal? LimitePorCartao,
        int? QuantidadeCartoes  
    ) : INotification;
}
