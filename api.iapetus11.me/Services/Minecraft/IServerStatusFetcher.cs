using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services.Minecraft;

public interface IServerStatusFetcher
{
    public Task<MinecraftServerStatus> FetchStatus();
}