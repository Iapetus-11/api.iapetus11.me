using api.iapetus11.me.Services.Minecraft;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
public class MinecraftServerController : Controller
{
    private readonly IServerStatusService _serverStatus;

    public MinecraftServerController(IServerStatusService statusService)
    {
        _serverStatus = statusService;
    }
    
    [Route("/mc/status/{server}")]
    public async Task<IActionResult> Status(string server)
    {
        return Json(await _serverStatus.FetchStatus(server));
    }
}