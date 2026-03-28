using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MiniERP.Application.DTOs;
using MiniERP.Application.Interfaces;

namespace MiniERP.Infrastructure.ExternalServices;

public class ReceitaWsClient : IReceitaWsClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReceitaWsClient> _logger;

    public ReceitaWsClient(HttpClient httpClient, ILogger<ReceitaWsClient> logger)
    {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri("https://receitaws.com.br/v1/cnpj/");
        
        _logger = logger;
    }

    public async Task<ConsultaCnpjResponseDto?> ConsultarCnpjAsync(string cnpj)
    {
        try
        {
            var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());

            var response = await _httpClient.GetAsync(cnpjLimpo);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao consultar CNPJ {Cnpj} na ReceitaWS. StatusCode: {Status}", cnpjLimpo, response.StatusCode);
                return null;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dados = await response.Content.ReadFromJsonAsync<ConsultaCnpjResponseDto>(options);

            if (dados != null && dados.Status?.Equals("ERROR", StringComparison.OrdinalIgnoreCase) == true)
            {
                _logger.LogWarning("ReceitaWS retornou erro lógico para o CNPJ {Cnpj}: {Message}", cnpjLimpo, dados.Message);
                return null;
            }

            return dados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro de exceção ao tentar consultar CNPJ {Cnpj} na ReceitaWS", cnpj);
            return null;
        }
    }
}