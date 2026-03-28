using MiniERP.Application.DTOs;
using MiniERP.Application.Interfaces;
using MiniERP.Domain.Exceptions;

namespace MiniERP.Application.UseCases;

public class GetCompanyDataByCnpjUseCase
{
    private readonly IReceitaWsClient _receitaWsClient;

    public GetCompanyDataByCnpjUseCase(IReceitaWsClient receitaWsClient)
    {
        _receitaWsClient = receitaWsClient;
    }

    public async Task<ConsultaCnpjResponseDto> ExecuteAsync(string cnpj)
    {
        DomainException.When(string.IsNullOrWhiteSpace(cnpj), "CNPJ não informado.");

        var dados = await _receitaWsClient.ConsultarCnpjAsync(cnpj);
        
        return dados ?? new ConsultaCnpjResponseDto();
    }
}