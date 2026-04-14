using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniERP.Domain.Entities;

namespace MiniERP.Infrastructure.Data.Mappings;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("Perfis");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Descricao).HasMaxLength(250);

        builder.HasData(
        new
        {
            Id = Guid.Parse("f0000000-0000-0000-0000-000000000001"),
            Nome = "Admin",
            Descricao = "Administrador do Sistema"
        },
        new
        {
            Id = Guid.Parse("f0000000-0000-0000-0000-000000000002"),
            Nome = "Gerente",
            Descricao = "Gestão do negócio e equipe"
        },
        new
        {
            Id = Guid.Parse("f0000000-0000-0000-0000-000000000003"),
            Nome = "Operador",
            Descricao = "Operação diária de vendas e clientes"
        });
    }
}