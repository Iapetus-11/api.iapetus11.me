using System.Collections.Immutable;
using api.iapetus11.me.Common;
using api.iapetus11.me.Models;
using DnsClient;
using DnsClient.Protocol;

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
    private const float _timeout = 2.5f;
    
    private static readonly ImmutableHashSet<char> _validAddressChars = "abcdefghijklmnopqrstuvwxyz0123456789.-_".ToImmutableHashSet();

    private readonly string _host;
    private readonly int _port;

    public MinecraftServerStatus Status { get; private set; }

    public MinecraftServer(string host, int port)
    {
        _host = host;
        _port = port;

        Status = DefaultStatus();
    }

    private static (string, int) ParseAddress(string address)
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

        if (port is not (> 0 and < 65535) && port != -1)
            throw new InvalidServerAddressException(address);

        if (host.ToLower().Any(c => !_validAddressChars.Contains(c)))
            throw new InvalidServerAddressException(address);

        return (host, port);
    }

    public static void TryParseAddress(string address, out string host, out int port, out bool invalidAddress)
    {
        try
        {
            (host, port) = ParseAddress(address);
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
            Array.Empty<MinecraftServerStatusPlayer>(), null, null, null, null, null, null);
    }

    private async Task<MinecraftServerStatus> FetchDefaultStatus()
    {
        await Task.Delay((int) (_timeout * 1000) + 1000);
        return DefaultStatus();
    }

    private async Task<(string?, int?)> DnsLookup()
    {
        var dnsClient = new LookupClient(new LookupClientOptions(NameServer.Cloudflare, NameServer.Cloudflare2)
        {
            UseTcpOnly = true,
            Timeout = TimeSpan.FromSeconds(_timeout)
        });

        SrvRecord? srvRecord;

        try
        {
            var result = await dnsClient.QueryAsync($"_minecraft._tcp.{_host}", QueryType.SRV);
            srvRecord = result.Answers.SrvRecords().OrderBy(r => r.Priority).FirstOrDefault();
        }
        catch (DnsResponseException)
        {
            return (null, null);
        }

        var host = (string?) srvRecord?.Target;
        var port = srvRecord?.Port;

        try
        {
            var result = await dnsClient.QueryAsync(host, QueryType.CNAME);
            var cname = (string?) result.Answers.CnameRecords().FirstOrDefault()?.CanonicalName;

            if (!string.IsNullOrEmpty(cname)) host = cname;
        }
        catch (DnsResponseException) { }

        return (host, port);
    }
    
    private async Task<MinecraftServerStatus?> FetchJavaStatusWithDns()
    {
        var (host, port) = await DnsLookup();

        if (host is not null && port is not null)
            return await new JavaServerStatusFetcher(host, port, _timeout).FetchStatusQuiet();

        return null;
    }

    public async Task FetchStatus()
    {
        var defaultStatusTask = FetchDefaultStatus();

        var status = await AsyncHelpers.FirstNotNull(defaultStatusTask!,
            new JavaServerStatusFetcher(_host, _port, _timeout).FetchStatusQuiet(),
            new BedrockServerStatusFetcher(_host, _port, _timeout).FetchStatusQuiet(),
            FetchJavaStatusWithDns());

        Status = status ?? defaultStatusTask.Result;
    }
}