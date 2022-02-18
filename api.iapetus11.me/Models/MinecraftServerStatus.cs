using System.Text.Json.Serialization;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace api.iapetus11.me.Models;

public record MinecraftServerStatusPlayer(string Username, string Id);

public record MinecraftServerStatusVersion(string Brand, string Software, int Protocol);

public record MinecraftServerStatus(string Host, int Port, bool Online, float Latency,
    [property: JsonPropertyName("online_players")] int OnlinePlayers,
    [property: JsonPropertyName("max_players")] int MaxPlayers, MinecraftServerStatusPlayer[] Players,
    MinecraftServerStatusVersion? Version, string? Motd, [property: JsonPropertyName("motd_clean")] string? MotdClean,
    string? Favicon, string? Map, [property: JsonPropertyName("gamemode")] string? GameMode);