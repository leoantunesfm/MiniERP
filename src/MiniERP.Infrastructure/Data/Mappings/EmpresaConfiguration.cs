using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniERP.Domain.Entities;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Infrastructure.Data.Mappings;

public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
        builder.ToTable("Empresas");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Cnpj)
            .HasConversion(
                cnpj => cnpj.Numero, 
                value => new Cnpj(value))
            .IsRequired()
            .HasMaxLength(14);
        
        builder.HasIndex(e => e.Cnpj).IsUnique();

        builder.Property(e => e.Cep)
            .HasConversion(
                cep => cep != null ? cep.Numero : null, 
                value => value != null ? new Cep(value) : null)
            .HasMaxLength(8);

        builder.Property(e => e.RazaoSocial).HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.NomeFantasia).HasMaxLength(200).IsRequired(false);
        
        builder.Property(e => e.Status).IsRequired();

        builder.HasMany(e => e.Documentos)
               .WithOne(d => d.Empresa)
               .HasForeignKey(d => d.EmpresaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}