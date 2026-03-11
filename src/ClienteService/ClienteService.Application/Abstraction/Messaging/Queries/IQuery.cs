using ClienteService.Domain.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Application.Abstraction.Messaging.Queries
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
        where TResponse : IResult;
}
