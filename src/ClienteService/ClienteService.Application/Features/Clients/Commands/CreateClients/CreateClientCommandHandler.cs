using AutoMapper;
using ClienteService.Application.Abstraction.Messaging.Commands;
using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Entities.Clients;
using ClienteService.Domain.Entities.Clients.InputCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Features.Clients.Commands.CreateClients
{
    public sealed class CreateClientCommandHandler(
        ILogger<CreateClientCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper) : ICommandHandler<CreateClientCommand, ClientResponse>
    {
        private readonly ILogger<CreateClientCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ClientResponse>> Handle(
            CreateClientCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando criação do cliente: {Nome}", request.Dto.Name);

                var validate = request.Dto.Validate();
                if (validate is not null)
                    return validate;

                var input = _mapper.Map<CreatedClientInputCommand>(request.Dto);

                // Valida se já existe o cliente no nosso sistema
                var existingClient = await _unitOfWork.Repository<Client>()
                    .GetAll().FirstOrDefaultAsync(c => c.Name == input.Name &&
                                                       c.Email == c.Email, cancellationToken);

                if (existingClient is not null)
                    return Result<ClientResponse>.
                        Failed(400, Resources.ExceptionResources.ExceptionResources.exception_client_exist);

                var client = Client.Create(input);
                client.MarkAsCreated();

                await _unitOfWork.Repository<Client>()
                    .CreateAsync(client, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                _logger.LogInformation("Cliente criado com sucesso: {Id}", client.Id);

                var response = _mapper.Map<ClientResponse>(client);
                return Result<ClientResponse>
                    .Success(response, 204);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro ao criar cliente");
                return Result<ClientResponse>.
                        Failed(400, Resources.ExceptionResources.ExceptionResources.exception_failed_function);
            }
        }
    }
}
