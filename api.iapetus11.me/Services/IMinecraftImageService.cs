namespace api.iapetus11.me.Services;

public interface IMinecraftImageService
{
    public Stream GenerateAchievement(string achievement);
    public Stream GenerateSplashScreen(string text);
}