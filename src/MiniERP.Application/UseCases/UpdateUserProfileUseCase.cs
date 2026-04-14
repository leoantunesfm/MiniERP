using MiniERP.Application.DTOs;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class UpdateUserProfileUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPerfilRepository _perfilRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserProfileUseCase(
        IUsuarioRepository usuarioRepository, 
        IPerfilRepository perfilRepository, 
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _perfilRepository = perfilRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid tenantId, Guid usuarioId, UpdateUserProfileRequestDto request)
    {
        var usuario = await _usuarioRepository.GetByIdAndTenantWithPerfisAsync(usuarioId, tenantId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado ou não pertence a esta empresa.");

        var perfil = await _perfilRepository.GetByIdAsync(request.PerfilId);
        if (perfil == null)
            throw new DomainException("O perfil selecionado não existe.");

        usuario.AtualizarPerfil(perfil.Id);
        
        await _unitOfWork.CommitAsync();
    }
}