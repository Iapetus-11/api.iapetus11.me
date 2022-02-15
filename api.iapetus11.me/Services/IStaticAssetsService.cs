using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace api.iapetus11.me.Services;

public interface IStaticAssetsService
{
    public Image StatusBackgroundImage { get; }
    public Image DefaultFaviconImage { get; }
    public Image AchievementBaseImage { get; }
    public Image SplashBaseImage { get; }
    public FontFamily MinecraftiaFontFamily { get; }
    public void CacheAllAssets();
}