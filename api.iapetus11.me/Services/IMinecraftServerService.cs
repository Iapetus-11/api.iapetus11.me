using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;

namespace api.iapetus11.me.Services;

public interface IMinecraftServerService
{
    public Task<MinecraftServerStatus> FetchServerStatus(string address);
    public Task<Stream> FetchServerImage(string address, string name);
    public Task<Stream> FetchServerFavicon(string address);
}