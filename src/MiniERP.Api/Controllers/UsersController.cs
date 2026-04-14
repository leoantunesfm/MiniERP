using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Application.DTOs;
using MiniERP.Application.UseCases;

namespace MiniERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly ListUsersByTenantUseCase _listUsersByTenantUseCase;
    private readonly UpdateUserProfileUseCase _updateUserProfileUseCase;
    private readonly DeactivateUserUseCase _deactivateUserUseCase;

    public UsersController(
        RegisterUserUseCase registerUserUseCase,
        ListUsersByTenantUseCase listUsersByTenantUseCase,
        UpdateUserProfileUseCase updateUserProfileUseCase,
        DeactivateUserUseCase deactivateUserUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _listUsersByTenantUseCase = listUsersByTenantUseCase;
        _updateUserProfileUseCase = updateUserProfileUseCase;
        _deactivateUserUseCase = deactivateUserUseCase;
    }

    private Guid GetTenantId()
    {
        var tenantIdClaim = User.FindFirst("tenantId")?.Value;
        if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
            throw new UnauthorizedAccessException("Tenant ID não encontrado no token ou em formato inválido.");
            
        return tenantId;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDto request)
    {
        var tenantId = GetTenantId();
        var response = await _registerUserUseCase.ExecuteAsync(tenantId, request);
        
        return Created(string.Empty, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var tenantId = GetTenantId();
        var users = await _listUsersByTenantUseCase.ExecuteAsync(tenantId);
        
        return Ok(users);
    }

    [HttpPut("{id:guid}/profile")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserProfileRequestDto request)
    {
        var tenantId = GetTenantId();
        await _updateUserProfileUseCase.ExecuteAsync(tenantId, id, request);
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var tenantId = GetTenantId();
        await _deactivateUserUseCase.ExecuteAsync(tenantId, id);
        
        return NoContent();
    }
}