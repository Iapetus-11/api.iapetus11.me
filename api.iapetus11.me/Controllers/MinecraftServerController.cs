using api.iapetus11.me.Services;
using api.iapetus11.me.Services.Minecraft;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
public class MinecraftServerController : Controller
{
    private readonly IMinecraftServerStatusService _serverStatus;

    public MinecraftServerController(IMinecraftServerStatusService statusService)
    {
        _serverStatus = statusService;
    }
    
    [HttpGet("/mc/status/{server}")]
    public async Task<IActionResult> Status(string server)
    {
        try
        {
            return Ok(await _serverStatus.FetchStatus(server));
        }
        catch (InvalidServerAddressException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/mc/status/{server}/image")]
    public async Task<IActionResult> StatusImage(string server, [FromQuery(Name = "customName")] string? customName = null)
    {
        var imageStream = await _serverStatus.FetchStatusImage(server, customName);
        return File(imageStream, "image/png");
    }
}