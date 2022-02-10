namespace api.iapetus11.me.Models;

public class MinecraftServerStatus
{
    public bool Online { get; }
    public float Latency { get; }
    public int OnlinePlayers { get; }
    public int MaxPlayers { get; }
    public MinecraftServerStatusPlayer[] Players { get; }
    public MinecraftServerStatusVersion Version { get; }
    public string MessageOfTheDay { get; }
    public string Favicon { get; }
    public string Map { get; }
    public string GameMode { get; }

    public MinecraftServerStatus(bool online, float latency, int onlinePlayers, int maxPlayers,
        MinecraftServerStatusPlayer[] players, MinecraftServerStatusVersion version, string messageOfTheDay, string favicon, string map, string gameMode)
    {
        Online = online;
        Latency = latency;
        OnlinePlayers = onlinePlayers;
        MaxPlayers = maxPlayers;
        Players = players;
        Version = version;
        MessageOfTheDay = messageOfTheDay;
        Favicon = favicon;
        Map = map;
        GameMode = gameMode;
    }
}