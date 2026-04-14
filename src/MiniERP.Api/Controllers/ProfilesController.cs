using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Application.UseCases;

namespace MiniERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfilesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] ListProfilesUseCase useCase)
    {
        var profiles = await useCase.ExecuteAsync();
        return Ok(profiles);
    }
}