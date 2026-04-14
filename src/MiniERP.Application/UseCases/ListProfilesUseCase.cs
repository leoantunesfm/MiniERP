using MiniERP.Application.DTOs;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Application.UseCases;

public class ListProfilesUseCase
{
    private readonly IPerfilRepository _perfilRepository;

    public ListProfilesUseCase(IPerfilRepository perfilRepository)
    {
        _perfilRepository = perfilRepository;
    }

    public async Task<IEnumerable<PerfilResponseDto>> ExecuteAsync()
    {
        var perfis = await _perfilRepository.GetAllAsync();
        return perfis.Select(p => new PerfilResponseDto(p.Id, p.Nome, p.Descricao));
    }
}