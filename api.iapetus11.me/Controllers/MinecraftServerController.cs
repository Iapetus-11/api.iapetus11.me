using api.iapetus11.me.Services;
using api.iapetus11.me.Services.Minecraft;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
[Route("mc/server")]
public class MinecraftServerController : ControllerBase
{
    private readonly IMinecraftServerService _serverService;

    public MinecraftServerController(IMinecraftServerService serverService) => _serverService = serverService;
    
    [HttpGet("status/{address}")]
    public async Task<IActionResult> GetServerStatus(string address)
    {
        try
        {
            return Ok(await _serverService.FetchServerStatus(address));
        }
        catch (InvalidServerAddressException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("status/{address}/image")]
    public async Task<IActionResult> GetServerStatusImage(string address, [FromQuery(Name = "customName")] string? customName = null)
    {
        return File(await _serverService.FetchServerImage(address, customName ?? address), "image/png");
    }
}