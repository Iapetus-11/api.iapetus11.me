using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace api.iapetus11.me.Controllers;

[ApiController]
[Route("s")]
public class LinkShortenerController : ControllerBase
{
    private readonly ILogger<LinkShortenerController> _log;
    private readonly ILinkShortenerService _linkShortener;
    
    public LinkShortenerController(ILogger<LinkShortenerController> log, ILinkShortenerService linkShortener)
    {
        _log = log;
        _linkShortener = linkShortener;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> ShortenedLinkRedirect(string slug)
    {
        var headers = HttpContext.Request.Headers ?? throw new InvalidOperationException();
        var userAgent = headers["User-Agent"].ToString();
        
        StringValues ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        headers.TryGetValue("X-Forwarded-For", out ipAddress);
        headers.TryGetValue("Host", out ipAddress);
        headers.TryGetValue("CF-Connecting-IP", out ipAddress);
        
        var redirect = await _linkShortener.GetRedirectUrl(slug, ipAddress, userAgent);

        if (redirect == null) return NotFound($"No suitable redirect found for {slug}");

        return Redirect(redirect);
    }
}