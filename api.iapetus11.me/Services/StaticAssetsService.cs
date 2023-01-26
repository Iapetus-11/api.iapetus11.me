using System.Text.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace api.iapetus11.me.Services;

public class StaticAssetsService : IStaticAssetsService
{
    private readonly FontCollection _fontCollection = new();
    
    public Image StatusBackgroundImage { get; private set; } = null!;
    public Image DefaultFaviconImage { get; private set; } = null!;
    public Image AchievementBaseImage { get; private set; } = null!;
    public Image SplashBaseImage { get; private set; } = null!;
    public FontFamily MinecraftiaFontFamily { get; private set; }
    public int[] FractalSeed { get; private set; }

    private readonly ILogger<StaticAssetsService> _log;

    public StaticAssetsService(ILogger<StaticAssetsService> log)
    {
        _log = log;
    }

    public void CacheAllAssets()
    {
        _log.LogInformation("Start loading static assets");
        
        StatusBackgroundImage = Image.Load("Content/Images/dirt_background.png");
        DefaultFaviconImage = Image.Load("Content/Images/unknown_pack.png");
        AchievementBaseImage = Image.Load("Content/Images/achievement.png");
        SplashBaseImage = Image.Load("Content/Images/splash.png");
        MinecraftiaFontFamily = _fontCollection.Add("Content/Fonts/Minecraftia.ttf");
        FractalSeed = JsonSerializer.Deserialize<int[]>(File.ReadAllText("Content/fractal_seed.json"))!;
        
        _log.LogInformation("End loading static assets");
    }
}