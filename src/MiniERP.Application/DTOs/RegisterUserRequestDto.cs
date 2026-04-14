namespace MiniERP.Application.DTOs;

public class RegisterUserRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid PerfilId { get; set; }
}