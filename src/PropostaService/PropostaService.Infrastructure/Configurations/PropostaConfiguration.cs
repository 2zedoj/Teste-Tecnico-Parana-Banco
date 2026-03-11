using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropostaService.Domain.Entities.Propostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Infrastructure.Configurations
{
    public class PropostaConfiguration : IEntityTypeConfiguration<Proposta>
    {
        public void Configure(EntityTypeBuilder<Proposta> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(u => u.ClientId)
              .IsRequired();

            builder.Property(u => u.Score)
              .IsRequired();

            builder.Property(u => u.Limite)
              .IsRequired();

            builder.Property(u => u.MaxCartoes)
              .IsRequired();

            builder.Property(u => u.Status)
              .IsRequired();

            builder.Property(u => u.CreatedAt)
              .IsRequired();

            builder.HasIndex(al => new { al.ClientId, al.Score, al.Status })
            .IsUnique();
        }
    }
}
