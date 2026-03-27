using System.Text.RegularExpressions;
using MiniERP.Domain.Exceptions;

namespace MiniERP.Domain.ValueObjects;

public record Cep
{
    public string Numero { get; }

    public Cep(string numero)
    {
        DomainException.When(string.IsNullOrWhiteSpace(numero), "CEP não pode ser vazio.");

        var apenasNumeros = Regex.Replace(numero, "[^0-9]", "");

        DomainException.When(apenasNumeros.Length != 8, "CEP deve conter exatamente 8 dígitos.");

        Numero = apenasNumeros;
    }

    public override string ToString() => Numero;
}