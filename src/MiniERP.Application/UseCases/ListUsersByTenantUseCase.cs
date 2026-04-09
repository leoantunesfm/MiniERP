using MiniERP.Application.DTOs;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class ListUsersByTenantUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListUsersByTenantUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync(Guid tenantId)
    {
        var usuarios = await _usuarioRepository.GetAllByTenantWithPerfisAsync(tenantId);

        return usuarios.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email.Endereco,
            Ativo = u.Ativo,
            PerfilId = u.UsuarioPerfis.FirstOrDefault()?.PerfilId ?? Guid.Empty,
            Perfil = u.UsuarioPerfis.FirstOrDefault()?.Perfil?.Nome ?? "Sem Perfil"
        });
    }
}