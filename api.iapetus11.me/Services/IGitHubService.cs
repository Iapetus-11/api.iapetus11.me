using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services;

public interface IGitHubService
{
    public bool IsValidUserName(string userName);
    public Task<int> GetUserEarnedStars(string userName);
    public Task<int> GetUserMergedPullRequests(string userName);
    public Task<int> GetUserOpenedIssues(string userName);
    public Task<IEnumerable<string>> GetUserDependantRepositories(string userName);
    public Task<string> GetUserEarnedStarsShieldSvg(string userName, ShieldQueryParams shieldParams);
    public Task<string> GetUserMergedPullRequestsShieldSvg(string userName, ShieldQueryParams shieldParams);
    public Task<string> GetUserOpenedIssuesShieldSvg(string userName, ShieldQueryParams shieldParams);
    public Task<string> GetUserDependantRepositoriesShieldSvg(string userName, ShieldQueryParams shieldParams);
}
