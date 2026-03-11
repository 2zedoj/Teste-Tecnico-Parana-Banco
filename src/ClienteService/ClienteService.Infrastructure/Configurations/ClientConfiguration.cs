using ClienteService.Domain.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Infrastructure.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(50);

            builder.OwnsOne(s => s.Document, n =>
            {
                n.Property(p => p.Value)
                 .HasColumnName("Document")
                 .HasMaxLength(20)
                 .IsRequired();
            });

            builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(320);

            builder.Property(u => u.Renda)
               .IsRequired();

            builder.Property(u => u.Score)
               .IsRequired();

            builder.HasIndex(al => new { al.Name, al.Email })
            .IsUnique();
        }
    }
}
