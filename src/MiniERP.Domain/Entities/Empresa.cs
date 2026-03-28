using MiniERP.Domain.Common;
using MiniERP.Domain.Enums;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Domain.Entities;

public class Empresa : BaseEntity
{
    public string? RazaoSocial { get; private set; }
    public string? NomeFantasia { get; private set; }
    public Cnpj Cnpj { get; private set; } = null!;
    public DateTime DataCadastro { get; private set; }

    public TenantStatus Status { get; private set; }

    public string? Telefone { get; private set; }
    public Cep? Cep { get; private set; }
    public string? Logradouro { get; private set; }
    public string? Numero { get; private set; }
    public string? Complemento { get; private set; }
    public string? Bairro { get; private set; }
    public string? Municipio { get; private set; }
    public string? Uf { get; private set; }

    public string? TokenConfirmacaoEmail { get; private set; }

    public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();
    public ICollection<DocumentoEmpresa> Documentos { get; private set; } = new List<DocumentoEmpresa>();

    protected Empresa() { }

    public Empresa(Cnpj cnpj)
    {
        Cnpj = cnpj;
        DataCadastro = DateTime.UtcNow;
        Status = TenantStatus.AguardandoConfirmacaoEmail;

        TokenConfirmacaoEmail = Guid.NewGuid().ToString("N");
    }

    public void CompletarCadastro(
        string razaoSocial, 
        string nomeFantasia, 
        string telefone,
        Cep cep, 
        string logradouro, 
        string numero, 
        string? complemento, 
        string bairro, 
        string municipio, 
        string uf)
    {
        DomainException.When(string.IsNullOrWhiteSpace(razaoSocial), "A Razão Social é obrigatória.");
        DomainException.When(string.IsNullOrWhiteSpace(nomeFantasia), "O Nome Fantasia é obrigatório.");
        
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Telefone = telefone;
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Municipio = municipio;
        Uf = uf;
    }

    public void AtivarTenant()
    {
        Status = TenantStatus.Ativo;
    }

    public void AguardarDadosCompletos()
    {
        Status = TenantStatus.AguardandoDadosCompletos;
    }

    public void ConfirmarEmail()
    {
        DomainException.When(Status != TenantStatus.AguardandoConfirmacaoEmail, "O e-mail já foi confirmado ou o status é inválido.");
        
        Status = TenantStatus.AguardandoDadosCompletos; 
        
        TokenConfirmacaoEmail = null; 
    }
}
