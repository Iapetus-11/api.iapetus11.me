using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;

namespace api.iapetus11.me.Services;

public class MinecraftServerStatusService : IMinecraftServerStatusService
{
    public async Task<MinecraftServerStatus> FetchStatus(string address)
    {
        return await new Server(address).FetchStatus();
    }

    public async Task<Stream> FetchStatusImage(string address, string? customName)
    {
        var server = new Server(address);

        if (string.IsNullOrWhiteSpace(customName))
            customName = server.Port != -1 ? $"{server.Host.TrimEnd('.')}:{server.Port}" : server.Host.TrimEnd('.');
        
        return await new Server(address).FetchStatusImage(customName);
    }
}