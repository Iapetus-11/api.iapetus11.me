using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services.Minecraft;

public interface IServerStatusService
{
    public Task<MinecraftServerStatus> FetchStatus(string address);
}