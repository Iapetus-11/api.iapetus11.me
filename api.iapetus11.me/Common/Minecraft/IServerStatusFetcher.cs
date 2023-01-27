using System.Net.Sockets;
using api.iapetus11.me.Models;

namespace api.iapetus11.me.Common.Minecraft;

public interface IServerStatusFetcher
{
    public Task<MinecraftServerStatus> FetchStatus();
    public Task<MinecraftServerStatus?> FetchStatusQuiet();
}