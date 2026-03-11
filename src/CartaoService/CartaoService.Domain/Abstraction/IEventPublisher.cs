using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Domain.Abstraction
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : class;
    }
}
