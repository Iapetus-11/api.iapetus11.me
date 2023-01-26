namespace Fractals;

public static class FractalFunctions
{
    public delegate (double, double) Function(double x, double y);

    private static (double, double) Linear(double x, double y) => (x, y);

    private static (double, double) Sine(double x, double y)
    {
        return (Math.Sin(x), Math.Sin(y));
    }

    private static (double, double) Spherical(double x, double y)
    {
        var r = 1.0 / Math.Pow(Math.Sqrt(x * x + y * y), 2);
        return (r * x, r * y);
    }

    private static (double, double) Horseshoe(double x, double y)
    {
        var r = Math.Sqrt(x * x + y * y);
        return ((1.0 / r) * (x - y) * (x + y), (2.0 * x * y) / r);
    }

    private static (double, double) Cross(double x, double y)
    {
        var k = Math.Sqrt(1.0 / Math.Pow(x * x - y * y, 2));
        return (k * x, k * y);
    }

    private static (double, double) Tangent(double x, double y)
    {
        return (Math.Sin(x) / Math.Cos(y), Math.Tan(y));
    }

    private static (double, double) Bubble(double x, double y)
    {
        var k = 4.0 / (Math.Pow(Math.Sqrt(x * x + y * y), 2) + 4.0);
        return (y * k, x * k);
    }

    private static (double, double) RadTan(double x, double y)
    {
        var r = Math.Sqrt(x * x + y * y);
        return (x, 1.0 / Math.Tan(r));
    }
    
    // private static (double, double) Heart(double x, double y)
    // {
    //     var theta = Math.Atan(x / y);
    //     var r = Math.Sqrt(x * x + y * y);
    //
    //     return (Math.Sin(theta * r) * r, r * -Math.Cos(theta * r));
    // }
    
    private static (double, double) Tangle(double x, double y)
    {
        var theta = Math.Atan(x / y);
        var r = Math.Sqrt(x * x + y * y);
        var oOR = 1.0 / r;

        return (oOR * (Math.Cos(theta) + Math.Sin(r)), oOR * (Math.Sin(theta) - Math.Cos(r)));
    }
    
    
    private static (double, double) Diamond(double x, double y)
    {
        var theta = Math.Atan(x / y);
        var r = Math.Sqrt(x * x + y * y);

        return (Math.Sin(theta) * Math.Cos(r), Math.Cos(theta) * Math.Sin(r));
    }

    public static Function GetFunction(FractalVariation variation) => variation switch
    {
        FractalVariation.Linear => Linear,
        FractalVariation.Sine => Sine,
        FractalVariation.Spherical => Spherical,
        FractalVariation.Horseshoe => Horseshoe,
        FractalVariation.Cross => Cross,
        FractalVariation.Tangent => Tangent,
        FractalVariation.Bubble => Bubble,
        FractalVariation.RadTan => RadTan,
        FractalVariation.Tangle => Tangle,
        FractalVariation.Diamond => Diamond,
        _ => throw new Exception("Invalid FractalVariation type specified"),
    };
}