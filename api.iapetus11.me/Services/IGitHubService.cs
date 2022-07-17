using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services;

public interface IGitHubService
{
    public bool IsValidUserName(string userName);
    public Task<int> GetUserEarnedStars(string userName);
    public Task<int> GetUserMergedPullRequests(string userName);
    public Task<int> GetUserOpenedIssues(string userName);
    public Task<string> GetUserEarnedStarsCardSvg(string userName, ShieldQueryParams shieldParams);
    public Task<string> GetUserMergedPullRequestsCardSvg(string userName, ShieldQueryParams shieldParams);
    public Task<string> GetUserOpenedIssuesCardSvg(string userName, ShieldQueryParams shieldParams);
}