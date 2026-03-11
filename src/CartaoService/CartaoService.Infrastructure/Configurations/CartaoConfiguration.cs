using CartaoService.Domain.Entities.Cartoes;
using CartaoService.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Infrastructure.Configurations
{
    public class CartaoConfiguration : IEntityTypeConfiguration<Cartao>
    {
        public void Configure(EntityTypeBuilder<Cartao> builder)
        {
            builder.ToTable("Cartoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ClienteId).IsRequired();
            builder.Property(c => c.PropostaId).IsRequired();
            builder.Property(c => c.ClienteNome).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Limite).HasPrecision(18, 2).IsRequired();
            builder.Property(c => c.Status).IsRequired();
            builder.Property(c => c.Validade).IsRequired();
            builder.Property(c => c.DataEmissao).IsRequired();

            builder.OwnsOne(c => c.Numero, n =>
            {
                n.Property(x => x.Valor)   // ← x aqui é NumeroCartao
                 .HasColumnName("Numero")
                 .HasMaxLength(16)
                 .IsRequired();
            });

            builder.OwnsOne(c => c.CVV, cvv =>
            {
                cvv.Property(x => x.Valor)  // ← x aqui é CVV
                   .HasColumnName("CVV")
                   .HasMaxLength(3)
                   .IsRequired();
            });
        }
    }
}
