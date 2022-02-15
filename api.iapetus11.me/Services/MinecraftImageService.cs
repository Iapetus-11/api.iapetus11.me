using api.iapetus11.me.Extensions;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace api.iapetus11.me.Services;

public class MinecraftImageService : IMinecraftImageService
{
    private static readonly PointF _splashTextPos = new PointF(556+5, 98+10);

    private readonly IStaticAssetsService _assets;

    public MinecraftImageService(IStaticAssetsService assets) => _assets = assets;
    
    public Stream GenerateAchievement(string achievement)
    {
        return _assets.AchievementBaseImage
            .Clone(x => x
                .DrawAdjustingText(achievement, 60, 54, _assets.MinecraftiaFontFamily, Color.White, 16, 250,
                    HorizontalAlignment.Left, out _))
            .ToPngStream();
    }

    public Stream GenerateSplashScreen(string text)
    {
        return _assets.SplashBaseImage.Clone(x => x
            .SetDrawingTransform(Matrix3x2Extensions.CreateRotation(-0.45f, _splashTextPos))
            .DrawAdjustingText(text, _splashTextPos.X, _splashTextPos.Y, _assets.MinecraftiaFontFamily,
                Color.ParseHex("3F3F00"), 25, 200,
                HorizontalAlignment.Center, out _)
            .DrawAdjustingText(text, _splashTextPos.X - 2, _splashTextPos.Y - 2, _assets.MinecraftiaFontFamily,
                Color.ParseHex("FFFF00"), 25, 200, HorizontalAlignment.Center, out _)
        ).ToPngStream();
    }
}