namespace MiniERP.Domain.Entities;

public class UsuarioPerfil
{
    public Guid UsuarioId { get; private set; }
    public Guid PerfilId { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Perfil Perfil { get; private set; } = null!;

    protected UsuarioPerfil() { }

    public UsuarioPerfil(Guid usuarioId, Guid perfilId)
    {
        UsuarioId = usuarioId;
        PerfilId = perfilId;
    }
}