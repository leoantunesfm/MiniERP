namespace MiniERP.Application.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public Guid PerfilId { get; set; }
    public bool Ativo { get; set; }
}