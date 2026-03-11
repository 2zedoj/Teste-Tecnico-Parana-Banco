using CartaoService.Domain.Abstraction;
using CartaoService.Domain.Entities.Cartoes;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Integration.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Application.Features.Cartoes
{
    public sealed class EmitirCartoesHandler
    : INotificationHandler<PropostaAprovadaIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmitirCartoesHandler> logger;

        public EmitirCartoesHandler(
            IUnitOfWork unitOfWork,
            ILogger<EmitirCartoesHandler> logger)
        {
            _unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task Handle(
            PropostaAprovadaIntegrationEvent notification,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Iniciando criação dos cartões: {Nome}", notification.ClienteNome);

                // QuantidadeCartoes vem da proposta (0, 1 ou 2)
                for (int i = 0; i < notification.QuantidadeCartoes; i++)
                {
                    var cartao = Cartao.Emitir(
                        notification.PropostaId,
                        notification.ClienteId,
                        notification.ClienteNome,
                        (decimal)notification.LimitePorCartao!
                    );

                    await _unitOfWork.Repository<Cartao>()
                        .CreateAsync(cartao, cancellationToken);

                    logger.LogInformation("Proposta criada com sucesso: {Id}", cartao.Id);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar os cartões");
            }
        }
    }
}
