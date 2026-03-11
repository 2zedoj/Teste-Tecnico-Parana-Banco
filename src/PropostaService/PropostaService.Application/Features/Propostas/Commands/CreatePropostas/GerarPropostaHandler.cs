using MediatR;
using Microsoft.Extensions.Logging;
using PropostaService.Domain.Abstraction;
using PropostaService.Domain.Entities.Propostas;
using PropostaService.Domain.Entities.Propostas.InputCommands;
using Shared.Integration.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Application.Features.Propostas.Commands.CreatePropostas
{
    public sealed class GerarPropostaHandler
        : INotificationHandler<ClientCreatedIntegrationEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GerarPropostaHandler> logger;

        public GerarPropostaHandler(
            ILogger<GerarPropostaHandler> logger,
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task Handle(
            ClientCreatedIntegrationEvent notification, 
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Iniciando criação da proposta: {Nome}", notification.ClientId);

                var proposta = Proposta.Create(
                    notification.ClientId,
                    notification.Name,
                    notification.Score);

                proposta.AnaliseProposta();

                proposta.MarkAsCreated();

                await unitOfWork.Repository<Proposta>()
                    .CreateAsync(proposta);

                await unitOfWork.CommitAsync(cancellationToken);

                logger.LogInformation("Proposta criada com sucesso: {Id}", proposta.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar a proposta");
            }
        }
    }
}
