using MiniERP.Application.DTOs;

namespace MiniERP.Application.Interfaces;

public interface IReceitaWsClient
{
    Task<ConsultaCnpjResponseDto?> ConsultarCnpjAsync(string cnpj);
}