using System.Text.Json;
using Fractals;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

const int resolution = 256;
// const int frameDelay = 10;
// const int frameCount = 50;

var seed = JsonSerializer.Deserialize<int[]>(File.ReadAllText("fractal_seed.json"))!;

var image = new FractalGenerator(seed, 8192, FractalVariation.RadTan, Color.ParseHex("0023FF"),
    Color.ParseHex("23FF00"), FractalColoringStrategy.Experimental, 0.875, 0.5,
    0, 0.75, 1500000, false).Generate();

// var gifMetadata = gif.Metadata.GetGifMetadata()!;
// gifMetadata.RepeatCount = 0;

// var frameMetadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
// frameMetadata.FrameDelay = 0;


// for (var i = 0; i < frameCount; i++)
// {
//     Console.Write($"\r{i+1}/{frameCount}");
//     
//     double completion = (i + 1) / (double)frameCount;
//
//     if (completion >= 0.5)
//     {
//         completion = 1.0 - (completion);
//     }
//
//     completion *= 2;
//     
//     var transform = 0.30 * completion + (0.75 - 0.10);
//     
//     // transform: 0.75
//     var frame = new FractalGenerator(seed, resolution, FractalVariation.RadTan, Color.ParseHex("0023FF"),
//         Color.ParseHex("23FF00"), FractalColoringStrategy.Experimental, 0.875, 0.5,
//         -0.15, transform, 200000, false).Generate();
//     
//     var frameMetadata = frame.Frames.RootFrame.Metadata.GetGifMetadata();
//     frameMetadata.FrameDelay = frameDelay;
//
//     // Add the color image to the gif.
//     gif.Frames.AddFrame(frame.Frames.RootFrame);
// }
//
// gif.SaveAsGif("/Users/miloi/Projects/api.iapetus11.me/out.gif");
