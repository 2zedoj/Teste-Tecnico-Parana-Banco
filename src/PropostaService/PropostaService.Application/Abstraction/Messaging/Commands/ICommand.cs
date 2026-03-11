using MediatR;
using PropostaService.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Application.Abstraction.Messaging.Commands
{
    public interface ICommand : IRequest<Result<NoContentDto>>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
        where TResponse : IResult;

    public interface IBaseCommand;
}
