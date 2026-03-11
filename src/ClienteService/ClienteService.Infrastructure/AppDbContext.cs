using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Entities.Clients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Infrastructure
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

        public DbSet<Client> Clients { get; set; } = null!;

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

            // salva no banco primeiro
            var result = await base.SaveChangesAsync(cancellationToken);

            // dispara os eventos
            foreach (var domainEvent in domainEvents)
                await _mediator!.Publish(domainEvent, cancellationToken);

            // ✅ limpa os eventos das ENTIDADES (não dos eventos!)
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
