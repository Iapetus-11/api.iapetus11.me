using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;
using DnsClient;

namespace api.iapetus11.me.Services.Minecraft;

public class InvalidServerAddressException : Exception { }

public enum ServerType
{
    Unknown, Java, Bedrock
}

public class Server
{
    private string _host;
    private int _port;
    private ServerType _type;

    public Server(string host, int port, ServerType type = ServerType.Unknown)
    {
        _host = host;
        _port = port;
        _type = type;
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
        _type = ServerType.Java; // only Java servers use SRV records, so we can set the type to Java
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        await DnsLookup();
        var statusTasks = new List<Task<MinecraftServerStatus>>();

        switch (_type)
        {
            case ServerType.Unknown or ServerType.Java:
                // statusTasks.Add(new JavaServerStatusFetcher(_host, _port).FetchStatus());
                break;
            case ServerType.Unknown or ServerType.Bedrock:
                statusTasks.Add(new BedrockServerStatusFetcher(_host, _port).FetchStatus());
                break;
        }

        while (statusTasks.Any())
        {
            var status = await Task.WhenAny(statusTasks);
            statusTasks.Remove(status);

            try
            {
                return await status;
            }
            catch (Exception e) { }
        }

        return new MinecraftServerStatus(
            false, -1f, 0, 0, new MinecraftServerStatusPlayer[] {}, 
            null, null, null, null, null);
    }
}