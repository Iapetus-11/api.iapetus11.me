using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using ImageExtensions = SixLabors.ImageSharp.ImageExtensions;

namespace api.iapetus11.me.Services.Minecraft;

public class ServerImage
{
    private readonly MinecraftServerStatus _status;
    private readonly IStaticAssetsService _assets;

    public ServerImage(MinecraftServerStatus status, IStaticAssetsService assets)
    {
        _status = status;
        _assets = assets;
    }

    private void DrawMotd(IImageProcessingContext ctx)
    {
        var motd = _status.Online ? _status.Motd ?? "A beautiful Minecraft server..." : "This server is offline.";

        var font = _assets.MinecraftiaFontFamily.CreateFont(22, FontStyle.Regular);
        var options = new TextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        
        ctx.SetGraphicsOptions(new GraphicsOptions {Antialias = false});

        var drawnPixels = 0.0f;
        var drawnPixelsVerti = 0.0f;
        var color = Color.ParseHex(MinecraftColor.GetColorByKey("white").Hex);

        for (var i = 0; i < motd.Length; i++)
        {
            if (motd[i] == '§' && i < motd.Length - 1) // formatting detected
            {
                try
                {
                    color = Color.ParseHex(MinecraftColor.GetColorByCode(motd[i+1].ToString()).Hex);
                }
                catch (KeyNotFoundException) {}

                i++; // skip the color / style char
            }
            else
            {
                if (motd[i] == '\n')
                {
                    drawnPixelsVerti += 27;
                    drawnPixels = 0;
                }

                options.Origin = new PointF(146 + drawnPixels, 108 + drawnPixelsVerti);
                ctx.DrawText(options, motd[i].ToString(), color);
                drawnPixels += TextMeasurer.Measure(motd[i].ToString(), options).Width;
            }
        }
    }

    public Image Generate(string name)
    {
        var image = _assets.StatusBackgroundImage.CloneAs<Rgba32>();

        var nameTextWidth = 0f;
        var playerTextWidth = 0f;

        var favicon = string.IsNullOrWhiteSpace(_status.Favicon)
            ? _assets.DefaultFaviconImage
            : api.iapetus11.me.Extensions.ImageExtensions.FromB64Png(_status.Favicon);
        
        favicon.Mutate(x => x
            .SetGraphicsOptions(new GraphicsOptions {Antialias = false})
            .Resize(128, 128, new NearestNeighborResampler()));

        image.Mutate(DrawMotd);

        image.Mutate(x => x.DrawAdjustingText(name, 146, 40, _assets.MinecraftiaFontFamily, Color.White, 22, 324,
            HorizontalAlignment.Left, out nameTextWidth));

        image.Mutate(x => x.DrawAdjustingText($"{_status.OnlinePlayers} / {_status.MaxPlayers}", 762, 40,
            _assets.MinecraftiaFontFamily, Color.White, 22, 999, HorizontalAlignment.Right, out playerTextWidth));

        image.Mutate(x => x.DrawAdjustingText($"{_status.Latency}ms",
            (int) ((146 + nameTextWidth + (762 - playerTextWidth)) / 2.0f), 40, _assets.MinecraftiaFontFamily,
            Color.White, 22, 324,
            HorizontalAlignment.Center, out _));
        
        image.Mutate(x => x.DrawImage(favicon, new Point(6, 6), 1.0f));
        
        image.Mutate(x => x.RoundCorners(4));
        
        if (favicon != _assets.DefaultFaviconImage) favicon.Dispose();

        return image;
    }
}