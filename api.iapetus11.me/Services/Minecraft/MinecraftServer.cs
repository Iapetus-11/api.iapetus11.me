using System.Collections.Immutable;
using System.Net.Sockets;
using api.iapetus11.me.Extensions;
using api.iapetus11.me.Models;
using DnsClient;

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

public class MinecraftServer
{
    private static readonly ImmutableHashSet<char> _validAddressChars = "abcdefghijklmnopqrstuvwxyz0123456789.-_".ToImmutableHashSet();

    private string _host;
    private int _port;

    public MinecraftServerStatus Status { get; private set; }

    public MinecraftServer(string host, int port)
    {
        _host = host;
        _port = port;

        Status = DefaultStatus();
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

        if (port is not (> 0 and < 65535) && port != -1) throw new InvalidServerAddressException(address);

        if (host.ToLower().Any(c => !_validAddressChars.Contains(c)))
        {
            throw new InvalidServerAddressException(address);
        }

        return new Tuple<string, int>(host, port);
    }

    public static void TryParseAddress(string address, out string host, out int port, out bool invalidAddress)
    {
        try
        {
            var (h, p) = ParseAddress(address);
            host = h;
            port = p;
            invalidAddress = false;
        }
        catch (Exception)
        {
            host = address;
            port = -1;
            invalidAddress = true;
        }
    }

    private MinecraftServerStatus DefaultStatus()
    {
        return new MinecraftServerStatus(_host, _port,false, -1f, 0, 0,
            new MinecraftServerStatusPlayer[] {}, null, null, null, null, null, null);
    }

    private async Task DnsLookup()
    {
        var result = await new LookupClient().QueryAsync($"_minecraft._tcp.{_host}", QueryType.SRV);
        var record = result.Answers.SrvRecords().FirstOrDefault();

        if (record == null) return;
        
        _host = record.Target;
        _port = record.Port;
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
            new JavaServerStatusFetcher(_host, _port).FetchStatus(),
            new BedrockServerStatusFetcher(_host, _port).FetchStatus()
        };

        while (statusTasks.Any())
        {
            var statusTask = await Task.WhenAny(statusTasks);
            statusTasks.Remove(statusTask);

            if (statusTask == defaultStatusTask) break;

            try
            {
                Status = await statusTask;
                break;
            }
            catch (SocketException) { }
            catch (IOException) { }
        }

        return Status ??= await defaultStatusTask;
    }
}