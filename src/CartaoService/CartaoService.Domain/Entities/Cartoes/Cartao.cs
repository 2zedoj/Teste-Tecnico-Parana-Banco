using CartaoService.Domain.Abstraction;
using CartaoService.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Domain.Entities.Cartoes
{
    public class Cartao : AggregateRoot
    {
        private Cartao(
        Guid propostaId,
        Guid clienteId,
        string clienteNome,
        NumeroCartao numero,
        CVV cvv,
        DateTime validade,
        decimal limite,
        CartaoStatus status)
        {
            PropostaId = propostaId;
            ClienteId = clienteId;
            ClienteNome = clienteNome;
            Numero = numero;
            CVV = cvv;
            Validade = validade;
            Limite = limite;
            Status = status;
            DataEmissao = DateTime.UtcNow;
        }

        protected Cartao() { }

        public Guid PropostaId { get; private set; }
        public Guid ClienteId { get; private set; }
        public string ClienteNome { get; private set; } = null!;
        public NumeroCartao Numero { get; private set; } = null!;
        public CVV CVV { get; private set; } = null!;
        public DateTime Validade { get; private set; }
        public decimal Limite { get; private set; }
        public CartaoStatus Status { get; private set; }
        public DateTime DataEmissao { get; private set; }

        public static Cartao Emitir(
            Guid propostaId,
            Guid clienteId,
            string clienteNome,
            decimal limite)
        {
            return new Cartao(
                propostaId,
                clienteId,
                clienteNome,
                NumeroCartao.Gerar(),              // Luhn 
                CVV.Gerar(),                        // 3 dígitos
                DateTime.UtcNow.AddYears(4),        // hoje + 4 anos
                limite,
                CartaoStatus.Ativo                  // sempre Ativo
            );
        }
    }

    public enum CartaoStatus
    {
        Ativo
    }
}
