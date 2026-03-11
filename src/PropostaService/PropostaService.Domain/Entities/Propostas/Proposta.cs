using PropostaService.Domain.Abstraction;
using PropostaService.Domain.Common.Events;
using PropostaService.Domain.Entities.Propostas.InputCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Domain.Entities.Propostas
{
    public class Proposta : AggregateRoot
    {
        private Proposta(
            Guid clientId,
            string name,
            int score)
        {
            ClientId = clientId;
            ClienteName = name;
            Score = score;
        }

        protected Proposta () { }

        public Guid ClientId { get; private set; }
        public string ClienteName { get; private set; } = null!;
        public int Score { get; private set; }
        public decimal Limite { get; private set; } = 0;
        public int MaxCartoes { get; private set; } = 0;
        public StatusType Status { get; private set; }

        public static Proposta Create(Guid clientId,
                                      string name,
                                      int score)
        {
            var proposta = new Proposta(clientId, name, score);

            proposta.Status = StatusType.Pendente;

            return proposta;
        }

        public void AnaliseProposta()
        {
            switch (Score)
            {
                case <= 100:
                    Status = StatusType.Recusada;
                    Limite = 0;
                    MaxCartoes = 0;
                    break;
                case > 100 and <= 500:
                    Status = StatusType.Aprovada;
                    Limite = 1000;
                    MaxCartoes = 1;
                    break;
                default:
                    Status = StatusType.Aprovada;
                    Limite = 5000;
                    MaxCartoes = 2;
                    break;
            }
        }

        public void MarkAsCreated()
            => AddDomainEvent(new PropostaCreatedEvent(this));

    }

    public enum StatusType
    {
        Pendente = 0,
        Aprovada = 1,
        Recusada = 2
    }
}
