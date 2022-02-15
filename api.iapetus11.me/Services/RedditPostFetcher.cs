using System.Collections.Immutable;
using api.iapetus11.me.Models;
using Newtonsoft.Json;

namespace api.iapetus11.me.Services;

public class RedditPostFetcher : IRedditPostFetcher
{
    private static readonly IReadOnlyDictionary<string, string> _subredditGroups = new Dictionary<string, string>
    {
        {"meme", "memes+me_irl+dankmemes+wholesomememes+prequelmemes+comedyheaven+marvelmemes"},
        {"cursedMinecraft", "CursedMinecraft"},
        {"greentext", "greentext"},
        {"comic", "comics"},
    };

    private static readonly ImmutableHashSet<string> _validMediaExtensions =
        new[] {".png", ".jpg", ".gif", "jpeg"}.ToImmutableHashSet();

    private static readonly Random _rand = new();
    
    private readonly HttpClient _http;

    private readonly Dictionary<string, RedditPost[]> _postGroups = new();
    private readonly Dictionary<string, List<string>> _lastPosts = new();
    private Timer _timer = null!;

    public RedditPostFetcher(HttpClient http) => _http = http;

    public RedditPost FetchRandomPost(string subredditGroup, string? requesterId)
    {
        var posts = _postGroups[subredditGroup];
        var post = posts[_rand.Next(0, posts.Length)];

        if (requesterId != null)
        {
            if (_lastPosts.ContainsKey(requesterId))
            {
                var lastRequesterPosts = _lastPosts[requesterId];

                for (var i = 0; i < 20; i++)
                {
                    if (lastRequesterPosts.Contains(post.Id)) break;
                    post = posts[_rand.Next(0, posts.Length)];
                }
                
                lastRequesterPosts.Add(post.Id);

                if (lastRequesterPosts.Count > 5) _lastPosts[requesterId] = lastRequesterPosts.Skip(1).ToList();
            }
            else
            {
                _lastPosts[requesterId] = new List<string> {post.Id};
            }
        }

        return post;
    }

    public bool IsValidGroup(string subredditGroup) => _subredditGroups.ContainsKey(subredditGroup);

    public string[] GetSubredditGroups() => _subredditGroups.Keys.ToArray();

    private async void BackgroundFetchPosts(object? state)
    {
        foreach (var (subredditGroup, subreddits) in _subredditGroups)
        {
            var res = await _http.GetAsync($"https://reddit.com/r/{subreddits}/hot/.json?limit=500");
            var data = JsonConvert.DeserializeObject<RedditListing>(await res.Content.ReadAsStringAsync());
            var posts = data.Data.Children
                .Select(p => p.Data)
                .SkipWhile(p =>
                    p.RemovalReason != null || p.IsVideo || p.Pinned || p.Stickied || !string.IsNullOrEmpty(p.Selftext))
                .TakeWhile(p => p.Url != null && _validMediaExtensions.Contains(p.Url[^4..]))
                .Select(p => new RedditPost(p.Id, p.Subreddit, p.Author, p.Title, p.Permalink, p.Url, (int) p.Ups,
                    (int) p.Downs, p.Over18, p.Spoiler))
                .ToArray();

            if (posts.Any()) _postGroups[subredditGroup] = posts;
        }
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(BackgroundFetchPosts, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}