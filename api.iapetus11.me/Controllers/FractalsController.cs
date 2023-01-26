using api.iapetus11.me.Models;
using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
[Route("fractals")]
public class FractalsController : ControllerBase
{
    private readonly IFractalsService _fractals;
    
    public FractalsController(IFractalsService fractals)
    {
        _fractals = fractals;
    }
    
    [HttpGet]
    public IActionResult GetFractal([FromQuery] FractalQueryParams fractalQueryParams)
    {
        return File(_fractals.GenerateFractal(fractalQueryParams), "image/png");
    }
}