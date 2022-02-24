namespace api.iapetus11.me.Services;

public interface ILinkShortenerService
{
    public Task<string?> GetRedirectUrl(string slug, string? ipAddress, string? userAgent);
}