using api.iapetus11.me.Data;
using Microsoft.EntityFrameworkCore;

namespace api.iapetus11.me.Services;

public class LinkShortenerService : ILinkShortenerService
{
    private readonly ILogger<LinkShortenerService> _log;
    private readonly AppDbContext _db;

    public LinkShortenerService(ILogger<LinkShortenerService> log, AppDbContext db)
    {
        _log = log;
        _db = db;
    }
    
    public async Task<string?> GetRedirectUrl(string slug, string ipAddress, string? userAgent)
    {
        slug = slug.ToLower();

        var record = await _db.Links.FirstOrDefaultAsync(l => l.Slug.ToLower() == slug);

        if (record == null)
        {
            _log.LogInformation("No suitable redirect found for {Slug}", slug);
            return null;
        }

        await _db.LinkHits.AddAsync(new AppDbContext.LinkHit
        {
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LinkHitAt = DateTime.UtcNow,
            LinkId = record.LinkId,
        });

        await _db.SaveChangesAsync();
        
        _log.LogInformation("Found redirect {Slug} -> {Url}", slug, record.Url);
        return record.Url;
    }
}