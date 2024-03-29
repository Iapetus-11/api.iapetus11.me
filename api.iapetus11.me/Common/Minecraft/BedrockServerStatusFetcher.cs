﻿using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;

namespace api.iapetus11.me.Common.Minecraft;

public class BedrockServerStatusFetcher : IServerStatusFetcher
{
    // request to ask bedrock server for status
    private static readonly byte[] _bedrockStatusRequest =
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 0, 254, 254, 254, 254, 253, 253, 253, 253, 18, 52, 86, 120};

    private readonly string _host;
    private readonly int _port;
    private readonly TimeSpan _timeout;

    public BedrockServerStatusFetcher(string host, int? port, float timeoutSeconds)
    {
        _host = host;
        _port = port is > 0 and < 65535 ? (int) port : 19132;
        _timeout = TimeSpan.FromSeconds(timeoutSeconds);
    }
    
    public async Task<MinecraftServerStatus> FetchStatus()
    {
        var stopwatch = new Stopwatch();
        byte[] data;
        
        using (var client = new UdpClient(_host, _port))
        {
            stopwatch.Start();
            
            await client.SendAsync(_bedrockStatusRequest);
            var result = await client.ReceiveAsync().WaitAsync(_timeout);
            
            stopwatch.Stop();
            
            // slice + decode length of upcoming data and then rest of data
            var dataLengthData = result.Buffer[33..35];
            if (BitConverter.IsLittleEndian) Array.Reverse(dataLengthData);
            data = result.Buffer[35..(35 + BitConverter.ToUInt16(dataLengthData))];
        }

        var splitData = Encoding.UTF8.GetString(data).Split(';');

        var motd = new ServerMotd(splitData[1]);

        return new MinecraftServerStatus(
            _host,
            _port,
            true,
            stopwatch.ElapsedMilliseconds,
            int.Parse(splitData[4]),
            int.Parse(splitData[5]),
            new MinecraftServerStatusPlayer[] { },
            new MinecraftServerStatusVersion(
                "Bedrock Edition", "Bedrock Edition", int.Parse(splitData[2])),
            motd.Motd,
            motd.MotdClean,
            null,
            splitData.Length > 7 ? splitData[7] : null,
            splitData.Length > 8 ? splitData[8] : null);
    }

    public async Task<MinecraftServerStatus?> FetchStatusQuiet()
    {
        try
        {
            return await FetchStatus();
        }
        catch (SocketException) { }
        catch (IOException) { }
        catch (TimeoutException) { }

        return null;
    }
}