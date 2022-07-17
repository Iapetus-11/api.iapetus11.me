using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace api.iapetus11.me.Models;

public record ShieldQueryParams
{
    [JsonPropertyName("color"), JsonProperty("color")]
    public string? Color { get; init; }

    [JsonPropertyName("style"), JsonProperty("style")]
    public string? Style { get; init; }

    [JsonPropertyName("logo"), JsonProperty("logo")]
    public string? Logo { get; init; }

    [JsonPropertyName("logoColor"), JsonProperty("logoColor")]
    public string? LogoColor { get; init; }

    [JsonPropertyName("logoWidth"), JsonProperty("logoWidth")]
    public string? LogoWidth { get; init; }

    [JsonPropertyName("link"), JsonProperty("link")]
    public string? Link { get; init; }
}