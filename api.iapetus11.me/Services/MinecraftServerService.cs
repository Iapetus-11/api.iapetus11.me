using api.iapetus11.me.Services.Minecraft;
using Microsoft.Extensions.Caching.Memory;

namespace api.iapetus11.me.Services;

public class MinecraftServerService : IMinecraftServerService
{
    private readonly IMemoryCache _memoryCache;

    public MinecraftServerService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public async Task<MinecraftServer> FetchServer(string address, bool suppressErrors = false)
    {
        MinecraftServer server;

        if (_memoryCache.TryGetValue(address, out server)) return server;

        server = new MinecraftServer(address);

        try
        {
            await server.FetchStatus();
        }
        catch (Exception)
        {
            if (!suppressErrors) throw;
        }

        if (server.Status?.Online == true) _memoryCache.Set(address, server, TimeSpan.FromMinutes(1));

        return server;
    }
}