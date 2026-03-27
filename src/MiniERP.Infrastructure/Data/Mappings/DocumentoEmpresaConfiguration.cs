using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniERP.Domain.Entities;

namespace MiniERP.Infrastructure.Data.Mappings;

public class DocumentoEmpresaConfiguration : IEntityTypeConfiguration<DocumentoEmpresa>
{
    public void Configure(EntityTypeBuilder<DocumentoEmpresa> builder)
    {
        builder.ToTable("DocumentosEmpresas");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(255);
        builder.Property(e => e.S3Path).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.DataUpload).IsRequired();
    }
}