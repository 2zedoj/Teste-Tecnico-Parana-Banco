using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Entities.Clients;

namespace ClienteService.Domain.Common.Events
{
    public sealed record CreateclientEventhandler(Client Client) : IDomainEvent;
}
