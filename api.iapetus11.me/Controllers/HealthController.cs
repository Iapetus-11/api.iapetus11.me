using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;


[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly ICacheTrackerService _cacheTrack;
    private readonly IRedditPostService _reddit;
    
    public HealthController(ICacheTrackerService cacheTrack, IRedditPostService reddit)
    {
        _cacheTrack = cacheTrack;
        _reddit = reddit;
    }
    
    [HttpGet]
    public IActionResult GetApiStatistics()
    {
        return Ok(new
        {
            CacheKeyCount = _cacheTrack.GetActiveCacheKeys().Count,
            RedditPostsCacheCount = _reddit.GetPostsCacheCount(),
            RedditLastPostsCacheCount = _reddit.GetLastPostsCacheCount(),
        });
    }
}