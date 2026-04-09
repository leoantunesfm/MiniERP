using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class DeactivateUserUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserUseCase(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid tenantId, Guid usuarioId)
    {
        var usuario = await _usuarioRepository.GetByIdAndTenantWithPerfisAsync(usuarioId, tenantId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado ou não pertence a esta empresa.");

        var isAdmin = usuario.UsuarioPerfis.Any(p => p.Perfil.Nome == "Admin");
        if (isAdmin)
            throw new DomainException("Não é possível inativar um usuário com perfil de Administrador.");

        usuario.Inativar();
        
        await _unitOfWork.CommitAsync();
    }
}