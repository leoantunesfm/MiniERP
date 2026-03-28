using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class ConfirmEmailUseCase
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailUseCase(IEmpresaRepository empresaRepository, IUnitOfWork unitOfWork)
    {
        _empresaRepository = empresaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(string token)
    {
        DomainException.When(string.IsNullOrWhiteSpace(token), "Token não informado.");

        var empresa = await _empresaRepository.GetByTokenAsync(token);
        DomainException.When(empresa == null, "Token de confirmação inválido ou expirado.");

        empresa!.ConfirmarEmail();

        var sucesso = await _unitOfWork.CommitAsync();
        DomainException.When(!sucesso, "Erro ao confirmar o e-mail. Tente novamente.");

        return true;
    }
}