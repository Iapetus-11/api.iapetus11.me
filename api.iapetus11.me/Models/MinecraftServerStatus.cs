using System.Text.Json.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace api.iapetus11.me.Models;

public class MinecraftServerStatusPlayer
{
    public string Username { get; }
    public string Id { get; }

    public MinecraftServerStatusPlayer(string username, string id)
    {
        Username = username;
        Id = id;
    }
}

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

public class MinecraftServerStatus
{   
    public string Host { get; }
    public int Port { get; }
    public bool Online { get; }
    public float Latency { get; }
    [JsonPropertyName("online_players")]
    public int OnlinePlayers { get; }
    [JsonPropertyName("max_players")]
    public int MaxPlayers { get; }
    public MinecraftServerStatusPlayer[] Players { get; }
    public MinecraftServerStatusVersion? Version { get; }
    public string? Motd { get; }
    public string? Favicon { get; }
    public string? Map { get; }
    [JsonPropertyName("gamemode")]
    public string? GameMode { get; }

    public MinecraftServerStatus(string host, int port, bool online, float latency, int onlinePlayers,
        int maxPlayers, MinecraftServerStatusPlayer[] players, MinecraftServerStatusVersion? version, string? motd,
        string? favicon, string? map, string? gameMode)

    {
        Host = host;
        Port = port;
        Online = online;
        Latency = latency;
        OnlinePlayers = onlinePlayers;
        MaxPlayers = maxPlayers;
        Players = players;
        Version = version;
        Motd = motd;
        Favicon = favicon;
        Map = map;
        GameMode = gameMode;
    }
}