using System.ComponentModel.DataAnnotations;

namespace api.iapetus11.me.Models;

using Fractals;

public record FractalQueryParams([Range(512, 2048)] int Resolution, FractalVariation Variation,
    [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$")] string ColorA,
    [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$")] string ColorB,
    FractalColoringStrategy Coloring, [Range(0.0, 5.0)] double IterTransformX, [Range(0.0, 5.0)] double IterTransformY,
    [Range(-1.0, 1.0)] double XShift, [Range(0.0, 5.0)] double Transform, [Range(1, 5000000)] int Iterations, bool Mirrored, [Range(1.0, 4.0)] float? Blur,
    [Range(1.0, 4.0)] float? Sharpen);