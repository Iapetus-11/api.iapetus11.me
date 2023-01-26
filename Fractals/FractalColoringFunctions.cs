using SixLabors.ImageSharp.PixelFormats;

namespace Fractals;


public class FractalColoringFunctions
{
    public delegate Rgb24 Function(double x, double y, int k, int iterations);
    
    private readonly Rgb24 _colorA, _colorB;

    internal FractalColoringFunctions(Rgb24 colorA, Rgb24 colorB)
    {
        _colorA = colorA;
        _colorB = colorB;
    }
    
    private static double Sigmoid(double x, double b, double o)
    {
        var xX = Math.Pow(b, x);
        return xX / (xX + o);
    }

    private Rgb24 Gradient(double x, double y, int k, int iterations)
    {
        var t = 1.0 - ((x + y) / 2.0);
        
        return new Rgb24(
            (byte)Math.Min(255.0, _colorA.R + t * _colorB.R),
            (byte)Math.Min(255.0, _colorA.G + t * _colorB.G),
            (byte)Math.Min(255.0, _colorA.B + t * _colorB.B)
        );
    }

    private Rgb24 SigmoidGradient(double x, double y, int k, int iterations)
    {
        var t = Sigmoid(1.0 - ((x + y) / 2.0),5000, 75);
        
        return new Rgb24(
            (byte)Math.Min(255.0, _colorA.R + t * _colorB.R),
            (byte)Math.Min(255.0, _colorA.G + t * _colorB.G),
            (byte)Math.Min(255.0, _colorA.B + t * _colorB.B)
        );
    }
    
    // private Rgb24 Experimental(double x, double y, int k, int iterations)
    // {
    //     var percentDone = Sigmoid(((double)k / iterations), 5000, 75);
    //     var percentNotDone = 1 - percentDone;
    //
    //     return new Rgb24(
    //         (byte)(_colorA.R * percentNotDone + _colorB.R * percentDone),
    //         (byte)(_colorA.G * percentNotDone + _colorB.G * percentDone),
    //         (byte)(_colorA.B * percentNotDone + _colorB.B * percentDone)
    //     );
    // }
    
    private Rgb24 Experimental(double x, double y, int k, int iterations)
    {
        var percentDone = Sigmoid(((double)k / iterations), 100, 60);
        var percentNotDone = 1 - percentDone;
    
        return new Rgb24(
            (byte)(_colorA.R * percentNotDone + _colorB.R * percentDone),
            (byte)(_colorA.G * percentNotDone + _colorB.G * percentDone),
            (byte)(_colorA.B * percentNotDone + _colorB.B * percentDone)
        );
    }
    
    public Function GetFunction(FractalColoringStrategy strategy) => strategy switch
    {
        FractalColoringStrategy.Gradient => Gradient,
        FractalColoringStrategy.SigmoidGradient => SigmoidGradient,
        FractalColoringStrategy.Experimental => Experimental,
        _ => throw new Exception("Invalid FractalColoringStrategy type specified"),
    };
}