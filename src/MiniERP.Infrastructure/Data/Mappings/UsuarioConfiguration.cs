using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniERP.Domain.Entities;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Infrastructure.Data.Mappings;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Nome).IsRequired().HasMaxLength(150);
        builder.Property(e => e.SenhaHash).IsRequired();

        builder.Property(e => e.Email)
            .HasConversion(
                email => email.Endereco,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(150);
            
        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Empresa)
               .WithMany(emp => emp.Usuarios)
               .HasForeignKey(e => e.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}