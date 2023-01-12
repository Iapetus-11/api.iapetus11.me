using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
[Route("reddit")]
public class RedditController : Controller
{
    private readonly IRedditPostService _reddit;

    public RedditController(IRedditPostService reddit) => _reddit = reddit;

    [HttpGet("{subredditGroup}")]
    public IActionResult GetRedditPost(string subredditGroup,
        [FromQuery(Name = "requesterId")] string? requesterId = null)
    {
        
        if (!_reddit.IsValidGroup(subredditGroup))
            return BadRequest("Invalid subreddit group, must be one of: " + string.Join(", ", _reddit.GetSubredditGroups()));
        
        return Ok(_reddit.FetchRandomPost(subredditGroup, requesterId));
    }
}