using api.iapetus11.me.Models;
using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
[Route("github/stats")]
public class GitHubStatsController : ControllerBase
{
    private readonly IGitHubService _gitHub;
    
    public GitHubStatsController(IGitHubService gitHub)
    {
        _gitHub = gitHub;
    }

    [HttpGet("{userName}")]
    public async Task<IActionResult> GetGitHubUserStats(string userName)
    {
        if (!_gitHub.IsValidUserName(userName))
        {
            return BadRequest("Invalid GitHub username provided.");
        }

        return Ok(new
        {
            EarnedStars = await _gitHub.GetUserEarnedStars(userName),
            MergedPRs = await _gitHub.GetUserMergedPullRequests(userName),
            OpenedIssues = await _gitHub.GetUserOpenedIssues(userName),
        });
    }

    [HttpGet("{userName}/card/stars")]
    public async Task<IActionResult> GetGitHubUserStarCard(string userName, [FromQuery] ShieldQueryParams shieldParams)
    {
        if (!_gitHub.IsValidUserName(userName))
        {
            return BadRequest("Invalid GitHub username provided.");
        }

        return Content(await _gitHub.GetUserEarnedStarsCardSvg(userName, shieldParams), "image/svg+xml");
    }

    [HttpGet("{userName}/card/prs")]
    public async Task<IActionResult> GetGitHubUserPRsCard(string userName, [FromQuery] ShieldQueryParams shieldParams)
    {
        if (!_gitHub.IsValidUserName(userName))
        {
            return BadRequest("Invalid GitHub username provided.");
        }

        return Content(await _gitHub.GetUserMergedPullRequestsCardSvg(userName, shieldParams), "image/svg+xml");
    }

    [HttpGet("{userName}/card/issues")]
    public async Task<IActionResult> GetGitHubUserIssuesCard(string userName, [FromQuery] ShieldQueryParams shieldParams)
    {
        if (!_gitHub.IsValidUserName(userName))
        {
            return BadRequest("Invalid GitHub username provided.");
        }

        return Content(await _gitHub.GetUserOpenedIssuesCardSvg(userName, shieldParams), "image/svg+xml");
    }
}