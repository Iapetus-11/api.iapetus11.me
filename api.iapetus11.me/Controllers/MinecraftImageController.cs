using api.iapetus11.me.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

[ApiController]
public class MinecraftImageController : Controller
{
    private readonly IMinecraftImageService _minecraftImageService;

    public MinecraftImageController(IMinecraftImageService minecraftImageService)
    {
        _minecraftImageService = minecraftImageService;
    }
    
    [HttpGet("/mc/image/achievement/{achievement}")]
    public IActionResult GetCustomAchievement(string achievement)
    {
        if (achievement.Length is < 1 or > 30) return BadRequest("Achievement text is not within 1-30 characters in length");
        return File(_minecraftImageService.GenerateAchievement(achievement), "image/png");
    }

    [HttpGet("/mc/image/splash/{text}")]
    public IActionResult GetCustomSplash(string text)
    {
        if (text.Length is < 1 or > 30) return BadRequest("Splash text is not within 1-30 characters in length");
        return File(_minecraftImageService.GenerateSplashScreen(text), "image/png");
    }
}