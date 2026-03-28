namespace MiniERP.Application.DTOs;

public record CompleteRegistrationRequestDto
{
    public Guid EmpresaId { get; init; }
    public string RazaoSocial { get; init; } = string.Empty;
    public string NomeFantasia { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Cep { get; init; } = string.Empty;
    public string Logradouro { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string? Complemento { get; init; }
    public string Bairro { get; init; } = string.Empty;
    public string Municipio { get; init; } = string.Empty;
    public string Uf { get; init; } = string.Empty;
}