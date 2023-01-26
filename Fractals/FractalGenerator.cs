namespace Fractals;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class FractalGenerator
{
    private readonly int[] _seed;
    private readonly int _resolution;
    private readonly FractalFunctions.Function _function;
    private readonly FractalColoringFunctions.Function _colorFunction;
    private readonly FractalCoordinateFunctions.Function _coordinateFunction;
    private readonly double _iterTransformX, _iterTransformY, _transform;
    private readonly int _iterations;
    private readonly bool _mirrored;
    private readonly double _xShift;

    public FractalGenerator(int[] seed, int resolution, FractalVariation variation, Rgb24 colorA, Rgb24 colorB,
        FractalColoringStrategy coloring, double iterTransformX, double iterTransformY,  double xShift,
        double transform, int iterations, bool mirrored)
    {
        _seed = seed;
        _resolution = resolution;
        _function = FractalFunctions.GetFunction(variation);
        _colorFunction = new FractalColoringFunctions(colorA, colorB).GetFunction(coloring);
        _coordinateFunction = new FractalCoordinateFunctions(resolution, mirrored).GetFunction();
        _iterTransformX = iterTransformX;
        _iterTransformY = iterTransformY;
        _xShift = xShift;
        _transform = transform;
        _iterations = iterations;
        _mirrored = mirrored;
    }

    public Image Generate()
    {
        var image = new Image<Rgb24>(_resolution, _resolution);

        var mat = new[]
        {
            new[] { _transform, 0, 0, 0, _transform, 0 },
            new[] { _transform, 0, _iterTransformX, 0, _transform, 0 },
            new[] { _transform, 0, 0, 0, _transform, _iterTransformY }
        };
        
        var x = 0.5;
        var y = 0.5;

        for (var k = 0; k < _iterations; k++)
        {
            var i = _seed[k];
            
            (x, y) = _function(mat[i][0] * x + mat[i][1] * y + mat[i][2], mat[i][3] * x + mat[i][4] * y + mat[i][5]);

            if (k <= 20) continue;
            
            var (pX, pY) = _coordinateFunction(x + _xShift, y);

            if (pX <= 0 || pY <= 0 || pX >= _resolution || pY >= _resolution) continue;
            
            var color = _colorFunction(x, y, k, _iterations);

            image[pX, pY] = color;

            if (_mirrored)
            {
                var (pX2, pY2) = _coordinateFunction(-x - _xShift, y);
                var (pX3, pY3) = _coordinateFunction(x + _xShift, -y);
                var (pX4, pY4) = _coordinateFunction(-x - _xShift, -y);
                
                image[pX2, pY2] =  image[pX3, pY3] = image[pX4, pY4] = color;
            }
        }

        return image;
    }
}