using MediatR;
using CartaoService.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Application.Abstraction.Messaging.Commands
{
    public interface ICommand : IRequest<Result<NoContentDto>>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
        where TResponse : IResult;

    public interface IBaseCommand;
}
