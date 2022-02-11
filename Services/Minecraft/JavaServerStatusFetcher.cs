using System.Buffers;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.iapetus11.me.Services.Minecraft;

internal class Buffer
{
    private List<byte> _buffer;

    public Buffer()
    {
        _buffer = new();
    }

    public Buffer(IEnumerable<byte> data)
    {
        _buffer = data.ToList();
    }

    public byte[] ToArray()
    {
        return _buffer.ToArray();
    }

    public void SetData(IEnumerable<byte> data)
    {
        _buffer = data.ToList();
    }

    public void Clear()
    {
        _buffer.Clear();
    }

    public int Length()
    {
        return _buffer.Count;
    }

    private static void CorrectByteOrder(byte[] data)
    {
        if (BitConverter.IsLittleEndian) Array.Reverse(data);
    }

    public byte[] Read(int n)
    {
        var data = _buffer.Take(n).ToArray();
        _buffer = _buffer.Skip(n).ToList();
        return data;
    }

    public byte ReadByte()
    {
        var b = _buffer.Take(1).First();
        _buffer = _buffer.Skip(1).ToList();
        return b;
    }

    public void Write(byte[] data)
    {
        _buffer = _buffer.Concat(data).ToList();
    }

    public void Write(Buffer data)
    {
        Write(data.ToArray());
    }

    public void Write(byte data)
    {
        _buffer.Add(data);
    }

    public int ReadVarInt()
    {
        var value = 0;
        var length = 0;

        while (true)
        {
            var currentByte = ReadByte();
            value |= (currentByte & 0x7F) << (length * 7);

            length += 1;

            if (length > 5) throw new Exception("VarInt is too big.");

            if ((currentByte & 0x80) != 0x80) break;
        }

        return value;
    }

    public void WriteVarInt(int valueSigned)
    {
        var value = (uint) valueSigned;
        
        while (true)
        {
            if ((value & ~0x7F) == 0)
            {
                Write((byte) value);
                return;
            }

            Write((byte) ((value & 0x7f) | 0x80));
            value >>= 7;
        }
    }

    public string ReadUtf8()
    {
        var length = ReadVarInt();
        var data = Read(length);

        return Encoding.UTF8.GetString(data);
    }

    public void WriteUtf8(string value)
    {
        WriteVarInt(value.Length);
        Write(Encoding.UTF8.GetBytes(value));
    }

    public string ReadAscii()
    {
        var data = new List<byte>();

        while (data.Count == 0 || data.TakeLast(1).First() != 0)
        {
            data.Add(ReadByte());
        }

        return Encoding.GetEncoding("ISO-8859-1").GetString(data.Take(data.Count - 1).ToArray());
    }

    public void WriteAscii(string value)
    {
        Write(Encoding.GetEncoding("ISO-8859-1").GetBytes(value));
        Write(0);
    }

    public short ReadShort()
    {
        var data = Read(2);
        CorrectByteOrder(data);
        return BitConverter.ToInt16(data);
    }

    public void WriteShort(short value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }
    
    public ushort ReadUShort()
    {
        var data = Read(2);
        CorrectByteOrder(data);
        return BitConverter.ToUInt16(data);
    }

    public void WriteUShort(ushort value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }

    public int ReadInt()
    {
        var data = Read(4);
        CorrectByteOrder(data);
        return BitConverter.ToInt32(data);
    }

    public void WriteInt(int value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }

    public uint ReadUInt()
    {
        var data = Read(4);
        CorrectByteOrder(data);
        return BitConverter.ToUInt32(data);
    }

    public void WriteUInt(uint value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }

    public long ReadLong()
    {
        var data = Read(8);
        CorrectByteOrder(data);
        return BitConverter.ToInt64(data);
    }

    public void WriteLong(long value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }

    public ulong ReadULong()
    {
        var data = Read(8);
        CorrectByteOrder(data);
        return BitConverter.ToUInt64(data);
    }

    public void WriteULong(ulong value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        Write(data);
    }
}


class JavaServerConnection
{
    private static readonly Random _rand = new Random();
    
    private readonly string _host;
    private readonly int _port;
    
    private TcpClient _client;
    private NetworkStream _stream;

    public JavaServerConnection(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public async Task Connect()
    {
        _client = new TcpClient();
        await _client.ConnectAsync(_host, _port);
        _stream = _client.GetStream();
    }

    public void Close()
    {
        if (_stream != null) _stream.Close();
        if (_client != null) _client.Close();
    }
    
    public async Task<byte[]> Read(int n)
    {
        var buffer = new byte[n];
        var read = 0;

        while (read < n)
        {
            read += await _stream.ReadAsync(buffer, read, n - read);
        }

        return buffer;
    }

    private async Task<Buffer> ReadPacket()
    {
        // TODO: replace with something that won't duplicate VarInt reading code
        var packetLength = 0;
        var varIntLength = 0;

        while (true)
        {
            var currentByte = (await Read(1)).First();
            packetLength |= (currentByte & 0x7F) << (varIntLength * 7);

            varIntLength += 1;

            if (varIntLength > 5) throw new Exception("VarInt is too big.");

            if ((currentByte & 0x80) != 0x80) break;
        }

        return new Buffer(await Read(packetLength));
    }

    public async Task WritePacket(Buffer data)
    {
        var buffer = new Buffer();
        
        buffer.WriteVarInt(data.Length());
        buffer.Write(data);
        
        await _stream.WriteAsync(buffer.ToArray());
    }

    public async Task SendHandShakePacket()
    {
        var buffer = new Buffer();
        
        buffer.WriteVarInt(0); // packet id
        buffer.WriteVarInt(47); // protocol
        buffer.WriteUtf8(_host);
        buffer.WriteUShort((ushort) _port);
        buffer.WriteVarInt(1); // packet state change (intention to query status later)

        await WritePacket(buffer);
    }

    public async Task<long> FetchPing()
    {
        var buffer = new Buffer();
        var stopwatch = new Stopwatch();
        var pingToken = _rand.NextInt64();
        
        buffer.WriteVarInt(1); // packet id for test ping
        buffer.WriteLong(pingToken);
        
        stopwatch.Start();

        await WritePacket(buffer);

        var response = await ReadPacket();

        stopwatch.Stop();

        if (response.ReadVarInt() != 1) return -1;
        if (response.ReadLong() != pingToken) return -1;

        return stopwatch.ElapsedMilliseconds;
    }

    public async Task<JObject> FetchStatus()
    {
        var buffer = new Buffer();
        buffer.WriteVarInt(0); // packet id

        await WritePacket(buffer);

        var response = await ReadPacket();

        if (response.ReadVarInt() != 0) throw new IOException("Invalid status packet response packet");

        return JObject.Parse(response.ReadUtf8());
    }
}

internal class MinecraftColor
{
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

    public static MinecraftColor ByKey(string name, string code, string hex)
    {
        return new MinecraftColor(null, name, code, hex);
    }

    public static MinecraftColor ByCode(string key, string name, string hex)
    {
        return new MinecraftColor(key, name, null, hex);
    }
}

internal class ServerMotd
{
    // key -> MinecraftColor
    private static readonly IReadOnlyDictionary<string, MinecraftColor> _mcColors = new Dictionary<string, MinecraftColor>()
    {
        {"dark_red", MinecraftColor.ByKey("Dark Red", "4", "AA0000")},
        {"red", MinecraftColor.ByKey("Red", "c", "FF5555")},
        {"gold", MinecraftColor.ByKey("Gold", "6", "FFAA00")},
        {"yellow", MinecraftColor.ByKey("Yellow", "e", "FFFF55")},
        {"dark_green", MinecraftColor.ByKey("Dark Green", "2", "00AA00")},
        {"green", MinecraftColor.ByKey("Green", "a", "55FF55")},
        {"aqua", MinecraftColor.ByKey("Aqua", "b", "55FFFF")},
        {"dark_aqua", MinecraftColor.ByKey("Dark Aqua", "3", "00AAAA")},
        {"dark_blue", MinecraftColor.ByKey("Dark Blue", "1", "0000AA")},
        {"blue", MinecraftColor.ByKey("Blue", "9", "5555FF")},
        {"light_purple", MinecraftColor.ByKey("Light Purple", "d", "FF55FF")},
        {"white", MinecraftColor.ByKey("White", "f", "FFFFFF")},
        {"gray", MinecraftColor.ByKey("Gray", "7", "AAAAAA")},
        {"dark_gray", MinecraftColor.ByKey("Dark Gray", "8", "555555")},
        {"black", MinecraftColor.ByKey("Black", "0", "000000")}
    };
    
    // mc code -> MinecraftColor
    // private static readonly IReadOnlyDictionary<string, MinecraftColor> _mcColorsCodes = new Dictionary<string, MinecraftColor>()
    // {
    //     {"4", MinecraftColor.ByCode("dark_red", "Dark Red", "AA0000")},
    //     {"c", MinecraftColor.ByCode("red", "Red", "FF5555")},
    //     {"6", MinecraftColor.ByCode("gold", "Gold", "FFAA00")},
    //     {"e", MinecraftColor.ByCode("yellow", "Yellow", "FFFF55")},
    //     {"2", MinecraftColor.ByCode("dark_green", "Dark Green", "00AA00")},
    //     {"a", MinecraftColor.ByCode("green", "Green", "55FF55")},
    //     {"b", MinecraftColor.ByCode("aqua", "Aqua", "55FFFF")},
    //     {"3", MinecraftColor.ByCode("dark_aqua", "Dark Aqua", "00AAAA")},
    //     {"1", MinecraftColor.ByCode("dark_blue", "Dark Blue", "0000AA")},
    //     {"9", MinecraftColor.ByCode("blue", "Blue", "5555FF")},
    //     {"d", MinecraftColor.ByCode("light_purple", "Light Purple", "FF55FF")},
    //     {"f", MinecraftColor.ByCode("white", "White", "FFFFFF")},
    //     {"7", MinecraftColor.ByCode("gray", "Gray", "AAAAAA")},
    //     {"8", MinecraftColor.ByCode("dark_gray", "Dark Gray", "555555")},
    //     {"0", MinecraftColor.ByCode("black", "Black", "000000")}
    // };
    
    private static Regex _hexCodeRegex = new Regex(
        @"^#(?:[0-9a-fA-F]{3}){1,2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    private JToken _motd;

    public ServerMotd(JToken motd)
    {
        _motd = motd;
    }

    public override string ToString()
    {
        if (_motd.Type == JTokenType.String) return _motd.Value<string>() ?? "";
        
        var motdOut = new StringBuilder();
        List<JToken> motdEntries;

        if (_motd.Type == JTokenType.Object)
        {
            motdEntries = _motd["extra"]?.ToList() ?? new();
        } else if (_motd.Type == JTokenType.Array)
        {
            motdEntries = _motd.ToList();
        }
        else throw new Exception($"Unsupported data type: {_motd.Type}");
        
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
                
                foreach (var mcColor in _mcColors.Values)
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
                    var code = _mcColors?[color]?.Code;
                    if (code != null) motdOut.Append($"§{code}");
                } catch (KeyNotFoundException e) {}
            }
        }

        return motdOut + (_motd["text"]?.Value<string>() ?? "");
    }
}

public class JavaServerStatusFetcher : IServerStatusFetcher
{
    private readonly string _host;
    private readonly int _port;

    public JavaServerStatusFetcher(string host, int port)
    {
        _host = host;
        _port = port is > 0 and < 65535 ? port : 25565;
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        var connection = new JavaServerConnection(_host, _port);
        long latency = -1;
        JObject statusData;
    
        await connection.Connect();

        try
        {
            await connection.SendHandShakePacket();
            statusData = await connection.FetchStatus();
            latency = await connection.FetchPing();
        }
        finally
        {
            connection.Close();
        }

        var players = statusData["players"]?["sample"]?.Select(p =>
            new MinecraftServerStatusPlayer(p["name"].Value<string>(), p["id"].Value<string>())).ToArray() ?? new MinecraftServerStatusPlayer[]{};

        return new MinecraftServerStatus(
            host: _host,
            port: _port,
            online: true,
            latency: latency, 
            onlinePlayers: statusData["players"]["online"].Value<int>(),
            maxPlayers: statusData["players"]["max"].Value<int>(),
            players: players,
            version: new MinecraftServerStatusVersion("Java Edition", statusData["version"]["name"].Value<string>(), statusData["version"]["protocol"].Value<int>()),
            messageOfTheDay: new ServerMotd(statusData["description"]).ToString(),
            favicon: statusData["favicon"].Value<string>(),
            map: null,
            gameMode: null
        );
    }
}