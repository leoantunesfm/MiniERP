using MiniERP.Domain.Common;

namespace MiniERP.Domain.Entities;

public class Perfil : BaseEntity
{
    public string Nome { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public ICollection<UsuarioPerfil> UsuarioPerfis { get; private set; } = new List<UsuarioPerfil>();
    protected Perfil() { }

    public Perfil(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}