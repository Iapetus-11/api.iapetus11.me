using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services;

public interface IMinecraftServerStatusService
{
    public Task<MinecraftServerStatus> FetchStatus(string address);
    public Task<Stream> FetchStatusImage(string address, string? customName);
}