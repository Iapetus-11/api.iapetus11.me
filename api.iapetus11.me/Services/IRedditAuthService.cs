namespace api.iapetus11.me.Services;

public interface IRedditAuthService
{
    public Task<string> GetAuthToken();
}
