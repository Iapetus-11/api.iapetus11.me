using api.iapetus11.me.Services.Minecraft;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace api.iapetus11.me.Services;

public class MinecraftServerService : IMinecraftServerService
{
    private readonly IAppCache _cache;

    public MinecraftServerService(IAppCache cache)
    {
        _cache = cache;
    }
    
    public async Task<MinecraftServer> FetchServer(string address, bool suppressErrors = false)
    {
        return await _cache.GetOrAddAsync(GetCacheKey(address), async () =>
        {
            try
            {
                var server = new MinecraftServer(address);
                await server.FetchStatus();
                return server;
            }
            catch (Exception)
            {
                if (!suppressErrors) throw;
            }

            return new MinecraftServer("someValidOfflineAddress");
        });
    }

    private static string GetCacheKey(string address) => $"MinecraftServer:{address}";
}