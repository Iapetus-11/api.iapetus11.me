using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace api.iapetus11.me.Extensions;

public static class ImageExtensions
{
    public static void DrawAdjustingText(this IImageProcessingContext ctx, string text, int x, int y, FontFamily fontFamily,
        Color color, float defaultSize, float maxWidth, HorizontalAlignment alignmentHoriz, out float textWidth)
    {
        var fontSize = defaultSize;
        var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
        var options = new TextOptions(font)
        {
            HorizontalAlignment = alignmentHoriz,
            VerticalAlignment = VerticalAlignment.Center,
            Origin = new PointF(x, y),
        };

        ctx.SetGraphicsOptions(new GraphicsOptions {Antialias = false});

        while (TextMeasurer.Measure(text, options).Width > maxWidth)
        {
            fontSize -= 0.1f;
            options.Font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
        }
        
        ctx.DrawText(options, text, color);

        textWidth = TextMeasurer.Measure(text, options).Width;
    }
    
    public static void RoundCorners(this IImageProcessingContext ctx, float radius)
    {
        var (width, height) = ctx.GetCurrentSize();
        var rect = new RectangularPolygon(-0.5f, -0.5f, radius, radius);

        var cornerTopLeft = rect.Clip(new EllipsePolygon(radius - 0.5f, radius - 0.5f, radius));

        var rightPos = width - cornerTopLeft.Bounds.Width + 1;
        var bottomPos = height - cornerTopLeft.Bounds.Height + 1;

        var cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
        var cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
        var cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

        var corners = new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);

        ctx.SetGraphicsOptions(new GraphicsOptions {
            Antialias = true,
            AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // punch out new colors
        });

        foreach (var c in corners) ctx = ctx.Fill(Color.Red, c);
    }

    public static Stream ToPngStream(this Image image)
    {
        var stream = new MemoryStream();
        
        image.SaveAsPng(stream);
        stream.Position = 0;
        
        return stream;
    }
}