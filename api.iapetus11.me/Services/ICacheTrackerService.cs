namespace api.iapetus11.me.Services;

public interface ICacheTrackerService
{
    public void AddCacheKey(string key);

    public List<string> GetActiveCacheKeys();
}