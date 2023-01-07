using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services;

public interface IRedditPostService : IHostedService, IDisposable
{
    public int GetPostsCacheCount();
    public int GetLastPostsCacheCount();
    public RedditPost FetchRandomPost(string subredditGroup, string? requesterId);
    public bool IsValidGroup(string subredditGroup);
    public string[] GetSubredditGroups();
}