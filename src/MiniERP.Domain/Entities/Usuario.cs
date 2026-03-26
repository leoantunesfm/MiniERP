using MiniERP.Domain.Common;

namespace MiniERP.Domain.Entities;

public class Usuario : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public bool Ativo { get; private set; }
    public Empresa Empresa { get; private set; } = null!;
    public ICollection<UsuarioPerfil> UsuarioPerfis { get; private set; } = new List<UsuarioPerfil>();

    protected Usuario() { }

    public Usuario(Guid empresaId, string nome, string email, string senhaHash)
    {
        EmpresaId = empresaId;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Ativo = true;
    }
}