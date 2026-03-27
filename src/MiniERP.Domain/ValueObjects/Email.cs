using MiniERP.Domain.Exceptions;

namespace MiniERP.Domain.ValueObjects;

public record Email
{
    public string Endereco { get; }

    public Email(string endereco)
    {
        DomainException.When(string.IsNullOrWhiteSpace(endereco) || !endereco.Contains('@'), "E-mail com formato inválido.");

        Endereco = endereco.ToLower().Trim();
    }

    public override string ToString() => Endereco;
}