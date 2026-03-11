using ClienteService.Domain.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Abstraction.Messaging.Commands
{
    public interface ICommand : IRequest<Result<NoContentDto>>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
        where TResponse : IResult;

    public interface IBaseCommand;
}
