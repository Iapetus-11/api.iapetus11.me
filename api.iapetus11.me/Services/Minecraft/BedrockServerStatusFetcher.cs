using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;

namespace api.iapetus11.me.Services.Minecraft;

public class BedrockServerStatusFetcher : IServerStatusFetcher
{
    // request to ask bedrock server for status
    private static readonly byte[] _bedrockStatusRequest =
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 0, 254, 254, 254, 254, 253, 253, 253, 253, 18, 52, 86, 120};

    private readonly string _host;
    private readonly int _port;

    public BedrockServerStatusFetcher(string host, int port)
    {
        _host = host;
        _port = port is > 0 and < 65535 ? port : 19132;
    }
    
    public async Task<MinecraftServerStatus> FetchStatus()
    {
        var client = new UdpClient(_host, _port);
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        await client.SendAsync(_bedrockStatusRequest);
        var result = await client.ReceiveAsync();
        
        stopwatch.Stop();
        
        var data = Encoding.UTF8.GetString(result.Buffer);
        var splitData = data.Split(';');

        return new MinecraftServerStatus(
            host: _host,
            port: _port,
            online: true,
            latency: stopwatch.ElapsedMilliseconds,
            onlinePlayers: int.Parse(splitData[4]),
            maxPlayers: int.Parse(splitData[5]),
            players: new MinecraftServerStatusPlayer[] { },
            version: new MinecraftServerStatusVersion(
                "Bedrock Edition", "Bedrock Edition", int.Parse(splitData[2])),
            motd: splitData[1],
            favicon: null,
            map: splitData[7],
            gameMode: splitData[8]);
    }
}