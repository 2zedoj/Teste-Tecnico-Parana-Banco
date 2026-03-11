using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Domain.Abstraction
{
    public abstract class BaseEntity
    {
        protected BaseEntity() { }

        protected BaseEntity(Guid id)
            => Id = id;

        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    }
}
