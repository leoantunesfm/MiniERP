using MiniERP.Domain.Common;

namespace MiniERP.Domain.Entities;

public class Empresa : BaseEntity
{
    public string RazaoSocial { get; private set; }
    public string NomeFantasia { get; private set; }
    public string Cnpj { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();

    protected Empresa() { }

    public Empresa(string razaoSocial, string nomeFantasia, string cnpj)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        DataCadastro = DateTime.UtcNow;
    }
}
