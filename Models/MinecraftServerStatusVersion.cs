namespace api.iapetus11.me.Models;

public class MinecraftServerStatusVersion
{
    public string Brand { get; }
    public string Software { get; }
    public int Protocol { get; }

    public MinecraftServerStatusVersion(string brand, string software, int protocol)
    {
        Brand = brand;
        Software = software;
        Protocol = protocol;
    }
}