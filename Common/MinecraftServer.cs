using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;

namespace api.iapetus11.me.Common;

public class MinecraftServer
{
    private static readonly byte[] BedrockStatusRequest =
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 0, 254, 254, 254, 254, 253, 253, 253, 253, 18, 52, 86, 120};

    public string Host { get; set; }
    public int Port { get; set; }

    public MinecraftServer(string host, int port)
    {
        Host = host;
        Port = port;
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        return await FetchBedrockStatus();
    }

    private async Task<MinecraftServerStatus> FetchBedrockStatus()
    {
        var client = new UdpClient(Host, Port);
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        await client.SendAsync(BedrockStatusRequest);
        var result = await client.ReceiveAsync();
        
        stopwatch.Stop();
        
        var data = Encoding.UTF8.GetString(result.Buffer);
        var splitData = data.Split(';');

        return new MinecraftServerStatus(
            online: true,
            latency: (float) stopwatch.ElapsedMilliseconds,
            onlinePlayers: Int32.Parse(splitData[4]),
            maxPlayers: Int32.Parse(splitData[5]),
            players: new MinecraftServerStatusPlayer[] { },
            version: new MinecraftServerStatusVersion(
                "Bedrock Edition", "Bedrock Edition", Int32.Parse(splitData[2])),
            messageOfTheDay: splitData[1],
            favicon: null,
            map: splitData[7],
            gameMode: splitData[8]);
    }
}