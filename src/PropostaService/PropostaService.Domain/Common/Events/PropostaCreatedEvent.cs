using PropostaService.Domain.Abstraction;
using PropostaService.Domain.Entities.Propostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Domain.Common.Events
{
    public sealed record PropostaCreatedEvent(Proposta Proposta) : IDomainEvent;
}
