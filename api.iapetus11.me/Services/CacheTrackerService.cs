using LazyCache;

namespace api.iapetus11.me.Services;

public class CacheTrackerService : ICacheTrackerService
{
    private readonly IAppCache _cache;
    
    private readonly HashSet<string> _keys = new();

    public CacheTrackerService(IAppCache cache)
    {
        _cache = cache;
    }
    
    public void AddCacheKey(string key) => _keys.Add(key);

    public List<string> GetActiveCacheKeys() => _keys.Where(k => _cache.Get<object>(k) is not null).ToList();
}