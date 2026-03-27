using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniERP.Domain.Entities;

namespace MiniERP.Infrastructure.Data.Mappings;

public class UsuarioPerfilConfiguration : IEntityTypeConfiguration<UsuarioPerfil>
{
    public void Configure(EntityTypeBuilder<UsuarioPerfil> builder)
    {
        builder.ToTable("UsuarioPerfis");
        builder.HasKey(up => new { up.UsuarioId, up.PerfilId });

        builder.HasOne(up => up.Usuario)
               .WithMany(u => u.UsuarioPerfis)
               .HasForeignKey(up => up.UsuarioId);

        builder.HasOne(up => up.Perfil)
               .WithMany(p => p.UsuarioPerfis)
               .HasForeignKey(up => up.PerfilId);
    }
}