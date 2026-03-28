namespace MiniERP.Application.DTOs;

public record ConsultaCnpjResponseDto
{
    public string? Nome { get; init; }
    public string? Fantasia { get; init; }
    public string? Logradouro { get; init; }
    public string? Numero { get; init; }
    public string? Complemento { get; init; }
    public string? Bairro { get; init; }
    public string? Municipio { get; init; }
    public string? Uf { get; init; }
    public string? Cep { get; init; }
    public string? Telefone { get; init; }

    public string? Status { get; init; }
    public string? Message { get; init; }
}