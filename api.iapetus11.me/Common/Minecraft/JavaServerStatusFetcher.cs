using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;
using Newtonsoft.Json.Linq;
using Serilog;

namespace api.iapetus11.me.Common.Minecraft;

internal class Buffer
{
    private List<byte> _buffer;

    public Buffer()
    {
        _buffer = new List<byte>();
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

internal class JavaServerConnection : IDisposable
{
    private static readonly Random _rand = new();
    
    private readonly string _host;
    private readonly int _port;
    
    private TcpClient? _client;
    private NetworkStream? _stream;

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

    public void Dispose()
    {
        _stream?.Close();
        _stream?.Dispose();
        
        _client?.Close();
        _client?.Dispose();
    }

    private async Task<byte[]> Read(int n)
    {
        if (_stream == null) throw new InvalidOperationException("Stream is null.");
        
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

    private async Task WritePacket(Buffer data)
    {
        if (_stream == null) throw new InvalidOperationException("Stream is null.");
        
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

public class JavaServerStatusFetcher : IServerStatusFetcher
{
    private readonly string _host;
    private readonly int _port;
    private readonly TimeSpan _timeout;

    public JavaServerStatusFetcher(string host, int? port, float timeoutSeconds)
    {
        _host = host;
        _port = port is > 0 and < 65535 ? (int) port : 25565;
        _timeout = TimeSpan.FromSeconds(timeoutSeconds);
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        long latency;
        JObject statusData;

        using (var connection = new JavaServerConnection(_host, _port))
        {
            await connection.Connect().WaitAsync(_timeout);
            await connection.SendHandShakePacket().WaitAsync(_timeout);
            statusData = await connection.FetchStatus().WaitAsync(_timeout);
            latency = await connection.FetchPing().WaitAsync(_timeout);
        }

        var players = statusData["players"]?["sample"]?.Select(p =>
            new MinecraftServerStatusPlayer(p["name"]!.Value<string>()!, p["id"]!.Value<string>()!)).ToArray() ?? Array.Empty<MinecraftServerStatusPlayer>();

        var motd = new ServerMotd(statusData["description"]);

        return new MinecraftServerStatus(
            _host,
            _port,
            true,
            latency,
            statusData["players"]?["online"]?.Value<int>() ?? 0,
            statusData["players"]?["max"]?.Value<int>() ?? 0,
            players,
            new MinecraftServerStatusVersion("Java Edition", statusData["version"]?["name"]?.Value<string>(),
                statusData["version"]?["protocol"]?.Value<int>()),
            motd.Motd,
            motd.MotdClean,
            statusData["favicon"]?.Value<string>(),
            null,
            null
        );
    }
    
    public async Task<MinecraftServerStatus?> FetchStatusQuiet()
    {
        try
        {
            return await FetchStatus();
        }
        catch (SocketException) { }
        catch (IOException) { }
        catch (TimeoutException) { }

        return null;
    }
}