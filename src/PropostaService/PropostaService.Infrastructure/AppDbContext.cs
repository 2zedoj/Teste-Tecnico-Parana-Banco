using MediatR;
using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Abstraction;
using PropostaService.Domain.Entities.Propostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private readonly IMediator? _mediator;

        public AppDbContext(DbContextOptions options) : base(options) { }

        public AppDbContext(
            DbContextOptions options,
            IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected AppDbContext() { }

        public DbSet<Proposta> Propostas { get; set; } = null!;

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            var aggregates = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = aggregates
                .SelectMany(e => e.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
                await _mediator!.Publish(domainEvent, cancellationToken);

            aggregates.ForEach(a => a.ClearDomainEvents());

            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
