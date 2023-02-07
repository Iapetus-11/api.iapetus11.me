using System.Reflection;
using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using Fractals;
using LazyCache;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.iapetus11.me.Services;

public class FractalsService : IFractalsService
{
    private readonly IStaticAssetsService _assets;
    private readonly IAppCache _cache;

    public FractalsService(IStaticAssetsService assets, IAppCache cache)
    {
        _assets = assets;
        _cache = cache;
    }

    private static string GetCacheKey(FractalQueryParams fractalQueryParams)
    {
        var objectDict = fractalQueryParams.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => $"{prop.GetValue(fractalQueryParams, null)}");
        
        return $"Fractal:{string.Join(',', objectDict.Values)}";
    }
    
    public byte[] GenerateFractal(FractalQueryParams fractalQueryParams)
    {
        return _cache.GetOrAdd(GetCacheKey(fractalQueryParams), () =>
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

            using var memoryStream = new MemoryStream();
            image.ToPngStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }, DateTimeOffset.UtcNow.AddSeconds(60));
    }
}