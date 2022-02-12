using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services.Minecraft;

public class ServerStatusService : IServerStatusService
{
    public async Task<MinecraftServerStatus> FetchStatus(string address)
    {
        return await new Server(address).FetchStatus();
    }
}