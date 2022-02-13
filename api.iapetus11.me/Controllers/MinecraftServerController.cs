using api.iapetus11.me.Services;
using api.iapetus11.me.Services.Minecraft;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
public class MinecraftServerController : Controller
{
    private readonly IMinecraftServerService _serverService;

    public MinecraftServerController(IMinecraftServerService serverService)
    {
        _serverService = serverService;
    }
    
    [HttpGet("/mc/status/{serverAddress}")]
    public async Task<IActionResult> Status(string serverAddress)
    {
        try
        {
            return Ok((await _serverService.FetchServer(serverAddress)).Status);
        }
        catch (InvalidServerAddressException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/mc/status/{serverAddress}/image")]
    public async Task<IActionResult> StatusImage(string serverAddress, [FromQuery(Name = "customName")] string? customName = null)
    {
        var server = await _serverService.FetchServer(serverAddress, true);
        return File(await server.FetchStatusImage(customName ?? serverAddress), "image/png");
    }
}