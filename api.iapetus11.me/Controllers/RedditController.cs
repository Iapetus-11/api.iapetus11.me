using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
public class RedditController : Controller
{
    private readonly IRedditPostFetcher _reddit;

    public RedditController(IRedditPostFetcher reddit)
    {
        _reddit = reddit;
    }
    
    [HttpGet("/reddit/{subredditGroup}")]
    public IActionResult GetRedditPost(string subredditGroup, [FromQuery(Name = "requesterId")] string? requesterId = null)
    {
        return Ok(_reddit.FetchRandomPost(subredditGroup, requesterId));
    }
}