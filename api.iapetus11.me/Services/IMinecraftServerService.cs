using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;

namespace api.iapetus11.me.Services;

public interface IMinecraftServerService
{
    // public Task<MinecraftServer> FetchServer(string address, bool suppressErrors = false);
    public Task<MinecraftServerStatus> FetchServerStatus(string address);
    public Task<Stream> FetchServerImage(string address, string name);
}