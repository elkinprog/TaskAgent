using Microsoft.EntityFrameworkCore;
using Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistencia.FluentConfig
{
    public class ClientConfig
    {
        public ClientConfig(EntityTypeBuilder<Client> entity) 
        {
            entity.ToTable("Client");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.NameUser).IsRequired().HasMaxLength(50);

            entity.Property(p => p.Position).IsRequired().HasMaxLength(50);

            entity.Property(p => p.Description).IsRequired().HasMaxLength(50);

            entity.Property(p => p.Date).IsRequired().HasColumnType("datetime");

            entity.Property(p => p.Commet).IsRequired().HasMaxLength(500);

            entity.Property(p => p.Timer).IsRequired().HasMaxLength(10);

        }   
    }
}
