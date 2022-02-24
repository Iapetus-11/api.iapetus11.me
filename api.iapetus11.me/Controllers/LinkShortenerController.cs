using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

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
        var remoteAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        
        var redirect = await _linkShortener.GetRedirectUrl(slug, remoteAddress, userAgent);

        if (redirect == null)
        {
            return NotFound($"No suitable redirect found for {slug}");
        }

        return Redirect(redirect);
    }
}