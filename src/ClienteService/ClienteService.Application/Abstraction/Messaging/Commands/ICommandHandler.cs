using ClienteService.Domain.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Abstraction.Messaging.Commands
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result<NoContentDto>>
        where TCommand : ICommand;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
        where TResponse : IResult;
}
