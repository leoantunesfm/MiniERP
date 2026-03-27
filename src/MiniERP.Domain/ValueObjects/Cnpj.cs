using System.Text.RegularExpressions;
using MiniERP.Domain.Exceptions;

namespace MiniERP.Domain.ValueObjects;

public record Cnpj
{
    public string Numero { get; }

    public Cnpj(string numero)
    {
        DomainException.When(string.IsNullOrWhiteSpace(numero), "CNPJ não pode ser vazio.");

        var apenasNumeros = Regex.Replace(numero, "[^0-9]", "");

        // adicionar a validação real do dígito verificador aqui
        DomainException.When(apenasNumeros.Length != 14, "CNPJ deve conter exatamente 14 dígitos.");
        
        Numero = apenasNumeros;
    }

    public override string ToString() => Numero;
}