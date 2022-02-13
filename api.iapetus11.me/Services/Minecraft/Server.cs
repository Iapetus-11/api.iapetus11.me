using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Sockets;
using api.iapetus11.me.Models;
using DnsClient;
using SixLabors.ImageSharp;

namespace api.iapetus11.me.Services.Minecraft;

public class ServerStatusException : Exception
{
    public ServerStatusException(string message) : base(message) {}
    public ServerStatusException(string message, Exception inner) : base(message, inner) {}
}

public class InvalidServerAddressException : ServerStatusException
{
    public string Address { get; }

    public InvalidServerAddressException(string address) : base($"Invalid server address: {address}")
    {
        Address = address;
    }

    public InvalidServerAddressException(string address, Exception inner) : base($"Invalid server address: {address}",
        inner)
    {
        Address = address;
    }
}

public class Server
{
    private static readonly ImmutableHashSet<char> _validAddressChars = "abcdefghijklmnopqrstuvwxyz0123456789.-_".ToImmutableHashSet();

    public string Host { get; private set; }
    public int Port { get; private set; }

    private MinecraftServerStatus? _status;

    public Server(string host, int port)
    {
        Host = host;
        Port = port;
    }

    public Server(string address)
    {
        try
        {
            (Host, Port) = ParseAddress(address);
        }
        catch (Exception e)
        {
            throw new InvalidServerAddressException(address, e);
        }
    }

    private static Tuple<string, int> ParseAddress(string address)
    {
        var addressSplit = address.Split(':');
        string host;
        var port = -1;
        
        switch (addressSplit.Length)
        {
            case 1:
                host = addressSplit[0];
                break;
            case 2:
                host = addressSplit[0];
                port = int.Parse(addressSplit[1]);
                break;
            default:
                throw new InvalidServerAddressException(address);
        }

        if (!(port is > 0 and < 65535) && port != -1) throw new InvalidServerAddressException(address);

        if (host.ToLower().Any(c => !_validAddressChars.Contains(c)))
        {
            throw new InvalidServerAddressException(address);
        }

        return new Tuple<string, int>(host, port);
    }

    private MinecraftServerStatus DefaultStatus()
    {
        return new MinecraftServerStatus(Host, Port,false, -1f, 0, 0,
            new MinecraftServerStatusPlayer[] {}, null, null, null, null, null);
    }

    private async Task DnsLookup()
    {
        var result = await new LookupClient().QueryAsync($"_minecraft._tcp.{Host}", QueryType.SRV);
        var record = result.Answers.SrvRecords().FirstOrDefault();

        if (record == null) return;
        
        Host = record.Target;
        Port = record.Port;
    }

    private async Task<MinecraftServerStatus> FetchDefaultStatus()
    {
        await Task.Delay(2000);
        return DefaultStatus();
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        await DnsLookup();
        
        var defaultStatusTask = FetchDefaultStatus();
        var statusTasks = new List<Task<MinecraftServerStatus>>()
        {
            defaultStatusTask,
            new JavaServerStatusFetcher(Host, Port).FetchStatus(),
            new BedrockServerStatusFetcher(Host, Port).FetchStatus()
        };

        while (statusTasks.Any())
        {
            var statusTask = await Task.WhenAny(statusTasks);
            statusTasks.Remove(statusTask);

            if (statusTask == defaultStatusTask) break;

            try
            {
                _status =  await statusTask;
                break;
            } catch (SocketException) {}
        }

        return _status ??= await defaultStatusTask;
    }

    public async Task<Stream> FetchStatusImage(string name)
    {
        try
        {
            if (_status == null) await FetchStatus();
        }
        catch (Exception)
        {
            _status = DefaultStatus();
        }

        var image = new ServerImage(_status ?? throw new InvalidOperationException()).Generate(name);
        var stream = new MemoryStream();
        
        await image.SaveAsPngAsync(stream);
        image.Dispose();

        stream.Position = 0;

        return stream;
    }
}