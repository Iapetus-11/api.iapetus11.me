using System.Text.Json.Serialization;
using Flurl.Http;

namespace api.iapetus11.me.Services;

public class RedditAuthService : IRedditAuthService
{
    private readonly IFlurlClient _http;
    private readonly ILogger<RedditAuthService> _log;
    private readonly IConfigurationSection _redditConfig;

    private string? _authToken;
    private long? _authTokenExpiresAt;

    private record RedditAuthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }
    }

    public RedditAuthService(IConfiguration config, IFlurlClient http, ILogger<RedditAuthService> log)
    {
        _redditConfig = config.GetRequiredSection("Reddit");
        _http = http;
        _log = log;
    }

    public async Task<string> GetAuthToken()
    {
        if (_authToken is not null && DateTimeOffset.UtcNow.ToUnixTimeSeconds() < _authTokenExpiresAt - 5)
        {
            return _authToken;
        }
        
        _log.LogInformation("Reddit access token expired, fetching a new one");

        var tokenData = await _http
            .Request("https://www.reddit.com/api/v1/access_token")
            .WithBasicAuth(_redditConfig.GetValue<string>("ClientId"),
                _redditConfig.GetValue<string>("ClientSecret"))
            .WithHeader("User-Agent", _redditConfig.GetValue<string>("UserAgent"))
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials"
            })
            .ReceiveJson<RedditAuthTokenResponse>();

        _log.LogInformation(
            "Fetched new Reddit access token which expires in {RedditAccessTokenExpiresIn} seconds",
            tokenData.ExpiresIn);
        
        _authToken = tokenData.AccessToken;
        _authTokenExpiresAt = tokenData.ExpiresIn + DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return _authToken;
    }
}
