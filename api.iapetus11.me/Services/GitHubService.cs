using System.Drawing;
using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using api.iapetus11.me.Models.GitHub;
using Flurl.Http;
using LazyCache;

namespace api.iapetus11.me.Services;

public class GitHubService : IGitHubService
{
    private readonly IFlurlClient _http;
    private readonly IAppCache _cache;
    private readonly ILogger<GitHubService> _log;
    private readonly ICacheTrackerService _cacheTrack;
    
    public GitHubService(IFlurlClient http, IAppCache cache, ILogger<GitHubService> log, ICacheTrackerService cacheTrack)
    {
        _http = http;
        _cache = cache;
        _log = log;
        _cacheTrack = cacheTrack;
    }

    public bool IsValidUserName(string userName)
    {
        if (userName.StartsWith("-") || userName.EndsWith("-")) return false;

        if (userName.Length > 39) return false;

        if (userName.Contains("--")) return false;

        if (userName.Any(c => !char.IsLetterOrDigit(c) && c != '-')) return false;

        return true;
    }
    
    private async Task<List<Repository>> GetUserRepositories(string userName)
    {
        var key = $"GitHubUserRepos:{userName}";
        _cacheTrack.AddCacheKey(key);

        return await _cache.GetOrAddAsync(key, async () =>
        {
            var repos = new List<Repository>();
            var i = 0;

            while (true)
            {
                i++;
                
                var newRepos = await _http
                    .Request($"https://api.github.com/users/{userName}/repos?page={i}")
                    .WithHeader("User-Agent", "api.iapetus11.me")
                    .GetJsonAsync<List<Repository>>();
                
                repos.AddRange(newRepos);

                if (newRepos.Count < 30) break;
            }
            
            _log.LogInformation("Fetched {RepoCount} GitHub repositories for user {User}", repos.Count, userName);

            return repos;
        }, DateTimeOffset.Now.AddMinutes(5));
    }

    private async Task<SearchResult> SearchIssues(string query, int perPage, int pages)
    {
        var key = $"IssueSearchResult:{query},{perPage},{pages}";
        _cacheTrack.AddCacheKey(key);
        
        return await _cache.GetOrAddAsync(key, async () =>
        {
            var items = new List<SearchItem>();
            var incompleteResults = false;
            var totalCount = 0L;

            for (var i = 0; i < pages; i++)
            {
                var res = await _http
                    .Request("https://api.github.com/search/issues")
                    .WithHeader("User-Agent", "api.iapetus11.me")
                    .SetQueryParam("q", query)
                    .SetQueryParam("per_page", perPage)
                    .SetQueryParam("page", i + 1)
                    .GetJsonAsync<SearchResult>();

                if (res.IncompleteResults) incompleteResults = true;
                totalCount = res.TotalCount;

                items.AddRange(res.Items);
            }
            
            _log.LogInformation("Fetched {ItemCount} items for issue search query {Query}", items.Count, query);

            return new SearchResult()
            {
                TotalCount = totalCount,
                IncompleteResults = incompleteResults,
                Items = items.ToArray()
            };
        }, DateTimeOffset.Now.AddMinutes(5));
    }

    public async Task<int> GetUserEarnedStars(string userName)
    {
        var repositories = await GetUserRepositories(userName);

        return (int) repositories.Select(r => r.StargazersCount).Sum();
    }

    public async Task<int> GetUserMergedPullRequests(string userName)
    {
        var searchResult = await SearchIssues($"author:{userName} is:pr is:merged", 1, 1);

        return (int) searchResult.TotalCount;
    }

    public async Task<int> GetUserOpenedIssues(string userName)
    {
        var searchResult = await SearchIssues($"author:{userName} is:issue", 1, 1);

        return (int) searchResult.TotalCount;
    }

    private async Task<string> BaseShieldSvg(string label, object value, ShieldQueryParams shieldParams)
    {
        var key = $"ShieldSvg:{label},{value},{shieldParams}";
        _cacheTrack.AddCacheKey(key);
        
        return await _cache.GetOrAddAsync(key, async () => await _http
            .Request("https://img.shields.io/static/v1")
            .SetQueryParam("label", label)
            .SetQueryParam("message", value)
            .SetShieldQueryParams(shieldParams)
            .GetStringAsync(), DateTimeOffset.Now.AddMinutes(2));
    }

    public async Task<string> GetUserEarnedStarsShieldSvg(string userName, ShieldQueryParams shieldParams)
    {
        return await BaseShieldSvg("Earned Stars", await GetUserEarnedStars(userName), shieldParams);
    }

    public async Task<string> GetUserMergedPullRequestsShieldSvg(string userName, ShieldQueryParams shieldParams)
    {
        return await BaseShieldSvg("Merged PRs", await GetUserMergedPullRequests(userName), shieldParams);
    }

    public async Task<string> GetUserOpenedIssuesShieldSvg(string userName, ShieldQueryParams shieldParams)
    {
        return await BaseShieldSvg("Opened Issues", await GetUserOpenedIssues(userName), shieldParams);
    }
}