using ClienteService.Domain.Abstraction;
using ClienteService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Infrastructure.UnitOfWorks
{
    public class UnitOfWork(
        AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        public async Task<string> CommitAsync(
            CancellationToken cancellationToken = default,
            bool checkForConcurrency = false)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException) when (checkForConcurrency)
            {
                return "A concurrency conflit ocurred while saving changes";
            }

            return string.Empty;
        }

        public IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : BaseEntity
            => new GenericRepository<TEntity>(_context);
    }
}
