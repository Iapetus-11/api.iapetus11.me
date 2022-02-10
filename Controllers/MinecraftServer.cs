using Microsoft.AspNetCore.Mvc;

namespace api.iapetus11.me.Controllers;

public class MinecraftServer : Controller
{
    public async Task<IActionResult> ViewMinecraftServerStatus(string server)
    {
        // parse server into host and port
        string host;
        int port = -1;

        var addressSplit = server.Split(':');

        switch (addressSplit.Length)
        {
            case 1:
            {
                host = addressSplit[0];
            }
            case 2:
            {
                host = addressSplit[0];
                port = Int32.Parse(addressSplit[1]);
            }
            default: throw new Exception("Invalid server address.");
        }
        
        return Json();
    }
}