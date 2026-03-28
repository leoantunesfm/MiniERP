using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class GetTenantByIdUseCase
{
    private readonly IEmpresaRepository _empresaRepository;

    public GetTenantByIdUseCase(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<object> ExecuteAsync(Guid id)
    {
        var empresa = await _empresaRepository.GetByIdAsync(id);
        DomainException.When(empresa == null, "Empresa não encontrada.");

        return new 
        { 
            Id = empresa!.Id, 
            Cnpj = empresa.Cnpj.Numero 
        };
    }
}