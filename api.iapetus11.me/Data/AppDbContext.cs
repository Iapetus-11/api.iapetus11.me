using Microsoft.EntityFrameworkCore;

namespace api.iapetus11.me.Data;

public class AppDbContext : DbContext
{
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkHit> LinkHits { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public class Link
    {
        public uint LinkId { get; set; }
        public string Slug { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class LinkHit
    {
        public uint LinkHitId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime LinkHitAt { get; set; }
        
        public uint LinkId { get; set; }
        public Link Link { get; set; }
    }
}