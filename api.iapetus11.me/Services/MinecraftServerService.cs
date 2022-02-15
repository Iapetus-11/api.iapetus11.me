using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;
using LazyCache;

namespace api.iapetus11.me.Services;

public class MinecraftServerService : IMinecraftServerService
{
    private readonly IAppCache _cache;
    private readonly IStaticAssetsService _assets;

    public MinecraftServerService(IAppCache cache, IStaticAssetsService assets)
    {
        _cache = cache;
        _assets = assets;
    }
    
    private async Task<MinecraftServer> FetchServer(string address, bool suppressErrors = false)
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

    public async Task<MinecraftServerStatus> FetchServerStatus(string address)
    {
        return (await FetchServer(address)).Status;
    }
    
    public async Task<Stream> FetchServerImage(string address, string name)
    {
        var status = await FetchServerStatus(address);

        using (var image = new ServerImage(status, _assets).Generate(name))
        {
            return image.ToPngStream();
        }
    }

    private static string GetCacheKey(string address) => $"MinecraftServer:{address}";
}