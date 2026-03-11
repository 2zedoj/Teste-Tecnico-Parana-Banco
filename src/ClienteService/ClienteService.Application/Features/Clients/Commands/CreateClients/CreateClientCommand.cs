using ClienteService.Application.Abstraction.Messaging.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Features.Clients.Commands.CreateClients;

public record CreateClientCommand(
    CreatedClientDto Dto) : ICommand<ClientResponse>;
