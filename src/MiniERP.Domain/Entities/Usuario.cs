using MiniERP.Domain.Common;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Domain.Entities;

public class Usuario : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public string Nome { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public bool Ativo { get; private set; }
    public Empresa Empresa { get; private set; } = null!;
    public ICollection<UsuarioPerfil> UsuarioPerfis { get; private set; } = new List<UsuarioPerfil>();

    protected Usuario() { }

    public Usuario(Guid empresaId, string nome, Email email, string senhaHash)
    {
        EmpresaId = empresaId;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Ativo = true;
    }

    public void Inativar()
    {
        Ativo = false;
    }

    public void AtualizarPerfil(Guid perfilId)
    {
        UsuarioPerfis.Clear();
        UsuarioPerfis.Add(new UsuarioPerfil(Id, perfilId));
    }
}