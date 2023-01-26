namespace Fractals;

public class FractalCoordinateFunctions
{
    public delegate (int, int) Function(double x, double y);

    private readonly int _resolution;
    private readonly bool _mirrored;

    public FractalCoordinateFunctions(int resolution, bool mirrored)
    {
        _resolution = resolution;
        _mirrored = mirrored;
    }

    private (int, int) RadialMirror(double x, double y)
    {
        return (
            (int)(((x + 1) / 2) * _resolution),
            (int)(((y + 1) / 2) * -_resolution + _resolution)
        );
    }

    private (int, int) Normal(double x, double y)
    {
        return ((int)(x * _resolution), (int)(y * _resolution));
    }

    // private (int, int) Center(double x, double y)
    // {
    //     var halfRes = _resolution / 2.0;
    //
    //     return (
    //         (int)(x * halfRes),
    //         (int)(y * halfRes)
    //     );
    // }

    // private (int, int) NormalWithOverflow(double x, double y)
    // {
    //     return ((int)((x % 1.0) * _resolution), (int)((y % 1.0) * _resolution));
    // }

    public Function GetFunction()
    {
        if (_mirrored)
        {
            return RadialMirror;
        }

        return Normal;
    }
}