﻿using System.Collections.Immutable;
using System.Text.Json;
using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using Flurl.Http;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace api.iapetus11.me.Services;

public class RedditPostService : IRedditPostService
{
    private static readonly IReadOnlyDictionary<string, string> _subredditGroups = new Dictionary<string, string>
    {
        {"meme", "memes+me_irl+dankmemes+wholesomememes+prequelmemes+comedyheaven+marvelmemes"},
        // {"cursedMinecraft", "CursedMinecraft"},
        {"greentext", "greentext"},
        {"comic", "comics"},
    };

    private static readonly ImmutableHashSet<string> _validMediaExtensions =
        new[] {".png", ".jpg", ".gif", "jpeg"}.ToImmutableHashSet();

    private static readonly Random _rand = new();
    
    private readonly IFlurlClient _http;
    private readonly IRedditAuthService _redditAuth;
    private readonly ILogger<RedditPostService> _log;

    private readonly Dictionary<string, RedditPost[]> _postGroups = new();
    private readonly Dictionary<string, List<string>> _lastPosts = new();
    private Timer _timer = null!;

    private readonly DateTime _lastClearTime;
    private readonly IConfigurationSection _redditConfig;

    public RedditPostService(IFlurlClient http, IRedditAuthService redditAuth, ILogger<RedditPostService> log, IConfiguration configuration)
    {
        _http = http;
        _redditAuth = redditAuth;
        _log = log;
        _lastClearTime = DateTime.Now;
        _redditConfig = configuration.GetRequiredSection("Reddit");
    }

    public int GetPostsCacheCount() => _postGroups.SelectMany(kv => kv.Value).Count();

    public int GetLastPostsCacheCount() => _lastPosts.Select(kv => kv.Value).Count();

    public RedditPost FetchRandomPost(string subredditGroup, string? requesterId)
    {
        var posts = _postGroups[subredditGroup];
        var post = posts[_rand.Next(0, posts.Length)];

        if (requesterId != null)
        {
            if (_lastPosts.ContainsKey(requesterId))
            {
                var lastRequesterPosts = _lastPosts[requesterId];

                for (var i = 0; i < 40; i++)
                {
                    if (!lastRequesterPosts.Any(p => p.Equals(post.Id))) break;
                    post = posts[_rand.Next(0, posts.Length)];
                }
                
                lastRequesterPosts.Add(post.Id);

                if (lastRequesterPosts.Count > 20) _lastPosts[requesterId] = lastRequesterPosts.Skip(1).ToList();
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

    private async Task<RedditPost[]> FetchSubredditPosts(string subreddits)
    {
        RedditListing? data;

        var authToken = await _redditAuth.GetAuthToken();

        try
        {
            data = await _http
                .Request($"https://www.reddit.com/r/{subreddits}/hot/.json?limit=500")
                .WithHeader("User-Agent", _redditConfig.GetValue<string>("UserAgent"))
                .WithHeader("Authentication", $"Bearer {authToken}")
                .GetJsonAsync<RedditListing>();
        }
        catch (Exception e)
        {
            _log.LogError(e, "error occurred while fetching and decoding data from Reddit");
            return Array.Empty<RedditPost>();
        }

        if (data?.Data is null)
        {
            _log.LogWarning("data from Reddit couldn't be decoded properly");
            return Array.Empty<RedditPost>();
        }

        var posts = data.Data.Children
            .Select(p => p.Data)
            .Where(p =>
                !(p.RemovalReason != null || p.IsVideo || p.Pinned || p.Stickied || !string.IsNullOrEmpty(p.Selftext)))
            .Where(p => p.Url != null && _validMediaExtensions.Contains(p.Url[^4..]))
            .Select(p => new RedditPost(p.Id, p.Subreddit, p.Author, p.Title, "https://reddit.com" + p.Permalink, p.Url!, (int) p.Ups,
                (int) p.Downs, p.Over18, p.Spoiler))
            .ToArray();

        return posts;
    }

    private async void BackgroundFetchPosts(object? state)
    {
        var newPostGroups = new Dictionary<string, RedditPost[]>();
        
        foreach (var (subredditGroup, subreddits) in _subredditGroups)
        {
            var posts = await FetchSubredditPosts(subreddits);

            for (var i = 0; i < 5; i++)
            {
                if (posts.Any()) break;
                
                _log.LogWarning("Retrying fetching of reddit posts for subreddits: {Subreddits}", subreddits);
                
                posts = await FetchSubredditPosts(subreddits);    
            }
            
            if (posts.Any()) newPostGroups[subredditGroup] = posts;
        }
        
        if (DateTime.Now > _lastClearTime.AddHours(6))
        {
            _lastPosts.Clear();
            _postGroups.Clear();
        }
        
        _postGroups.UpdateWith(newPostGroups);
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(BackgroundFetchPosts, null, TimeSpan.Zero, TimeSpan.FromHours(2));
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
