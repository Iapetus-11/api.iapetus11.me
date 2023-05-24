using System.Text.Json;
using Fractals;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

string? GetArg(string arg)
{
    for (var i = 0; i < args.Length; i++)
    {
        if (args[i] == arg)
        {
            return args[i + 1];
        }
    }

    return null;
}

bool GetArgAsBoolean(string arg)
{
    return args.Contains(arg);
}

int GetArgAsInt(string arg, int defaultValue)
{
    if (GetArg(arg) is { } a)
    {
        return int.Parse(a);
    }

    return defaultValue;
}

double GetArgAsDouble(string arg, double defaultValue)
{
    if (GetArg(arg) is { } a)
    {
        return double.Parse(a);
    }

    return defaultValue;
}

var seed = JsonSerializer.Deserialize<int[]>(File.ReadAllText("fractal_seed.json"))!;

var resolution = GetArgAsInt("--resolution", 1024);
var variation = Enum.Parse<FractalVariation>(GetArg("--variation")!);
var colorA = Color.Parse(GetArg("--color-a"));
var colorB = Color.Parse(GetArg("--color-b"));
var coloringStrategy = Enum.Parse<FractalColoringStrategy>(GetArg("--coloring")!);
var iterTransformX = GetArgAsDouble("--iter-transform-x", 0.5);
var iterTransformY = GetArgAsDouble("--iter-transform-y", 0.5);
var xShift = GetArgAsDouble("--x-shift", 0.5);
var transform = GetArgAsDouble("--transform", 0.5);
var iterations = GetArgAsInt("--iterations", 500_000);
var mirrored = GetArgAsBoolean("--mirror");

var fractalGenerator = new FractalGenerator(seed, resolution, variation, colorA, colorB, coloringStrategy,
    iterTransformX, iterTransformY, xShift, transform, iterations, mirrored);

var image = fractalGenerator.Generate();

image.SaveAsPng("output.png");
