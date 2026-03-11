using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Domain.Abstraction
{
    public interface IUnitOfWork
    {
        Task<string> CommitAsync(
            CancellationToken cancellationToken = default,
            bool checkForConcurrency = false);

        IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : BaseEntity;
    }
}
