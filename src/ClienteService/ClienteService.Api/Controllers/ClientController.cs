using ClienteService.Application.Features.Clients;
using ClienteService.Application.Features.Clients.Commands.CreateClients;
using ClienteService.Domain.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClienteService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController(
        ISender sender) : BaseController
    {

        private readonly ISender _sender = sender;

        /// <summary>
        /// Create client.
        /// </summary>
        /// <param name="request"> It is mandatory to send the client's documented data.</param>
        /// <response code="500">Unexpected internal error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<ClientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ClientResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClientAsync(
            CreatedClientDto request,
            CancellationToken cancellationToken = default)
        {
            var response = await _sender.Send(
                new CreateClientCommand(request),
                cancellationToken);

            return CreateResult(response);
        }

    }
}
