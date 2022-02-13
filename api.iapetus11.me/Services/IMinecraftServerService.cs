using api.iapetus11.me.Services.Minecraft;

namespace api.iapetus11.me.Services;

public interface IMinecraftServerService
{
    public Task<MinecraftServer> FetchServer(string address, bool suppressErrors = false);
}