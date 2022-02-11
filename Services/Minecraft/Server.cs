using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;
using DnsClient;

namespace api.iapetus11.me.Services.Minecraft;

public class InvalidServerAddressException : Exception { }

public class Server
{
    private string _host;
    private int _port;

    public Server(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public Server(string address)
    {
        (_host, _port) = ParseAddress(address);
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
                throw new InvalidServerAddressException();
        }

        return new Tuple<string, int>(host, port);
    }

    private async Task DnsLookup()
    {
        var result = await new LookupClient().QueryAsync(_host, QueryType.SRV);
        var record = result.Answers.SrvRecords().FirstOrDefault();

        if (record == null) return;
        
        _host = record.Target;
        _port = record.Port;
        // _type = ServerType.Java; // only Java servers use SRV records, so we can set the type to Java
    }

    public async Task<MinecraftServerStatus> FetchDefaultStatus()
    {
        await Task.Delay(2000);
        
        return new MinecraftServerStatus(
            _host, _port, false, -1f, 0, 0, new MinecraftServerStatusPlayer[] {}, 
            null, null, null, null, null);
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
                return await statusTask;
            } catch (Exception e) when (e is SocketException) {}
        }
        
        return await defaultStatusTask;
    }
}