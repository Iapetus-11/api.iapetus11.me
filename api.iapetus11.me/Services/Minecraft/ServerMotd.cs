using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace api.iapetus11.me.Services.Minecraft;

public class MinecraftColor
{
    // key -> MinecraftColor
    private static readonly IReadOnlyDictionary<string, MinecraftColor> _mcColors = new Dictionary<string, MinecraftColor>
    {
        {"dark_red", ByKey("Dark Red", "4", "AA0000")},
        {"red", ByKey("Red", "c", "FF5555")},
        {"gold", ByKey("Gold", "6", "FFAA00")},
        {"yellow", ByKey("Yellow", "e", "FFFF55")},
        {"dark_green", ByKey("Dark Green", "2", "00AA00")},
        {"green", ByKey("Green", "a", "55FF55")},
        {"aqua", ByKey("Aqua", "b", "55FFFF")},
        {"dark_aqua", ByKey("Dark Aqua", "3", "00AAAA")},
        {"dark_blue", ByKey("Dark Blue", "1", "0000AA")},
        {"blue", ByKey("Blue", "9", "5555FF")},
        {"light_purple", ByKey("Light Purple", "d", "FF55FF")},
        {"white", ByKey("White", "f", "FFFFFF")},
        {"gray", ByKey("Gray", "7", "AAAAAA")},
        {"dark_gray", ByKey("Dark Gray", "8", "555555")},
        {"black", ByKey("Black", "0", "000000")}
    };
    
    // mc code -> MinecraftColor
    private static readonly IReadOnlyDictionary<string, MinecraftColor> _mcColorsByCode = new Dictionary<string, MinecraftColor>
    {
        {"4", ByCode("dark_red", "Dark Red", "AA0000")},
        {"c", ByCode("red", "Red", "FF5555")},
        {"6", ByCode("gold", "Gold", "FFAA00")},
        {"e", ByCode("yellow", "Yellow", "FFFF55")},
        {"2", ByCode("dark_green", "Dark Green", "00AA00")},
        {"a", ByCode("green", "Green", "55FF55")},
        {"b", ByCode("aqua", "Aqua", "55FFFF")},
        {"3", ByCode("dark_aqua", "Dark Aqua", "00AAAA")},
        {"1", ByCode("dark_blue", "Dark Blue", "0000AA")},
        {"9", ByCode("blue", "Blue", "5555FF")},
        {"d", ByCode("light_purple", "Light Purple", "FF55FF")},
        {"f", ByCode("white", "White", "FFFFFF")},
        {"7", ByCode("gray", "Gray", "AAAAAA")},
        {"8", ByCode("dark_gray", "Dark Gray", "555555")},
        {"0", ByCode("black", "Black", "000000")}
    };
    
    public string? Key { get; }
    public string Name { get; }
    public string? Code { get; }
    public string Hex { get; }

    public MinecraftColor(string? key, string name, string? code, string hex)
    {
        Key = key;
        Name = name;
        Code = code;
        Hex = hex;
    }

    private static MinecraftColor ByKey(string name, string code, string hex)
        => new(null, name, code, hex);

    private static MinecraftColor ByCode(string key, string name, string hex)
        => new(key, name, null, hex);

    public static MinecraftColor GetColorByKey(string key) => _mcColors[key];

    public static MinecraftColor GetColorByCode(string code) => _mcColorsByCode[code];

    public static IEnumerable<MinecraftColor> GetColors() => _mcColors.Values;
}

public class ServerMotd
{
    
    private static readonly Regex _hexCodeRegex = new(
        @"^#(?:[0-9a-fA-F]{3}){1,2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly string _motd;

    public ServerMotd(string motd) => _motd = motd;

    public ServerMotd(JToken? motd) => _motd = ParseJsonMotd(motd);

    public override string ToString() => _motd;

    private static string ParseJsonMotd(JToken? motdJson)
    {
        if (motdJson == null) return "";
        
        if (motdJson.Type == JTokenType.String) return motdJson.Value<string>() ?? "";
        
        var motdOut = new StringBuilder();

        List<JToken> motdEntries = motdJson.Type switch
        {
            JTokenType.Object => motdJson["extra"]?.ToList() ?? new(),
            JTokenType.Array => motdJson.ToList(),
            _ => throw new Exception($"Unsupported data type: {motdJson.Type}")
        };

        foreach (var entry in motdEntries)
        {
            if (entry["bold"]?.Value<bool>() == true) motdOut.Append("§l");
            if (entry["italic"]?.Value<bool>() == true) motdOut.Append("§o");
            if (entry["underlined"]?.Value<bool>() == true) motdOut.Append("§n");
            if (entry["obfuscated"]?.Value<bool>() == true) motdOut.Append("§k");

            var color = entry["color"]?.Value<string>();

            if (string.IsNullOrEmpty(color)) continue;
            
            // some servers don't use mc color codes, they use hex
            if (_hexCodeRegex.IsMatch("#" + color.Replace("#", "")))
            {
                var hexNice = color.ToUpper().Replace("#", "");
                
                foreach (var mcColor in MinecraftColor.GetColors())
                {
                    if (hexNice == mcColor.Hex)
                    {
                        motdOut.Append($"§{mcColor.Code}");
                        break;
                    }
                }
            }
            else // isn't a hex code, should be a normal mc color code
            {
                try
                {
                    motdOut.Append($"§{MinecraftColor.GetColorByKey(color).Code}");
                }
                catch (KeyNotFoundException) {}
            }

            var text = entry["text"]?.Value<string>();
            if (text != null) motdOut.Append(text);
        }

        return motdOut + (motdJson["text"]?.Value<string>() ?? "");
    }
}