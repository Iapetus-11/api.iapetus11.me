using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using Fractals;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.iapetus11.me.Services;

public class FractalsService : IFractalsService
{
    private readonly IStaticAssetsService _assets;

    public FractalsService(IStaticAssetsService assets)
    {
        _assets = assets;
    }
    
    public Stream GenerateFractal(FractalQueryParams fractalQueryParams)
    {
        using var image = new FractalGenerator(
            _assets.FractalSeed,
            fractalQueryParams.Resolution,
            fractalQueryParams.Variation,
            Color.ParseHex(fractalQueryParams.ColorA),
            Color.ParseHex(fractalQueryParams.ColorB),
            fractalQueryParams.Coloring,
            fractalQueryParams.IterTransformX,
            fractalQueryParams.IterTransformY,
            fractalQueryParams.XShift,
            fractalQueryParams.Transform,
            fractalQueryParams.Iterations,
            fractalQueryParams.Mirrored
        ).Generate();

        if (fractalQueryParams.Blur is { } blur)
            image.Mutate(ctx => ctx.GaussianBlur(blur));

        if (fractalQueryParams.Sharpen is { } sharpen)
            image.Mutate(ctx => ctx.GaussianSharpen(sharpen));

        return image.ToPngStream();
    }
}