using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services;

public interface IFractalsService
{
    public byte[] GenerateFractal(FractalQueryParams fractalQueryParams);
}