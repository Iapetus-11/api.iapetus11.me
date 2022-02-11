using System.Net.Sockets;
using System.Text;
using api.iapetus11.me.Models;
using api.iapetus11.me.Services.Minecraft;

namespace api.iapetus11.me.Services.Minecraft;

class JavaServerConnection
{
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
        
    }

    public static void CorrectByteOrder(byte[] data)
    {
        if (BitConverter.IsLittleEndian) Array.Reverse(data);
    }

    public async Task<byte[]> ReadExactly(int n)
    {
        var buffer = new byte[n];
        var read = 0;

        while (read < n)
        {
            read += await _stream.ReadAsync(buffer, read, n - read);
        }

        return buffer;
    }

    public async Task Write(byte[] data)
    {
        await _stream.WriteAsync(data);
    }

    private async Task<byte> ReadByte()
    {
        return (await ReadExactly(1))[0];
    }

    private async Task WriteByte(byte b)
    {
        await Write(new[] {b});
    }

    public async Task<int> ReadVarInt()
    {
        var value = 0;
        var length = 0;

        while (true)
        {
            var currentByte = await ReadByte();
            value |= (currentByte & 0x7F) << (length * 7);

            length += 1;

            if (length > 5) throw new Exception("VarInt is too big.");

            if ((currentByte & 0x80) != 0x80) break;
        }

        return value;
    }

    public async Task WriteVarInt(int valueSigned)
    {
        var value = (uint) valueSigned;
        
        while (true)
        {
            if ((value & ~0x7F) == 0)
            {
                await WriteByte((byte) value);
                return;
            }

            await WriteByte((byte) ((value & 0x7f) | 0x80));
            value >>= 7;
        }
    }

    public async Task<string> ReadUtf8()
    {
        var length = await ReadVarInt();
        var data = await ReadExactly(length);

        return Encoding.UTF8.GetString(data);
    }

    public async Task WriteUtf8(string value)
    {
        await WriteVarInt(value.Length);
        await Write(Encoding.UTF8.GetBytes(value));
    }

    public async Task<string> ReadAscii()
    {
        var data = new List<byte>();

        while (data.Count == 0 || data.TakeLast(1).ToArray()[0] != 0)
        {
            data.Add(await ReadByte());
        }

        return Encoding.GetEncoding("ISO-8859-1").GetString(data.Take(data.Count - 1).ToArray());
    }

    public async Task WriteAscii(string value)
    {
        await Write(Encoding.GetEncoding("ISO-8859-1").GetBytes(value));
        await WriteByte(0);
    }

    public async Task<short> ReadShort()
    {
        var data = await ReadExactly(2);
        CorrectByteOrder(data);
        return BitConverter.ToInt16(data);
    }

    public async Task WriteShort(short value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }
    
    public async Task<ushort> ReadUShort()
    {
        var data = await ReadExactly(2);
        CorrectByteOrder(data);
        return BitConverter.ToUInt16(data);
    }

    public async Task WriteUShort(ushort value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }

    public async Task<int> ReadInt()
    {
        var data = await ReadExactly(4);
        CorrectByteOrder(data);
        return BitConverter.ToInt32(data);
    }

    public async Task WriteInt(int value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }

    public async Task<uint> ReadUInt()
    {
        var data = await ReadExactly(4);
        CorrectByteOrder(data);
        return BitConverter.ToUInt32(data);
    }

    public async Task WriteUInt(uint value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }

    public async Task<long> ReadLong()
    {
        var data = await ReadExactly(8);
        CorrectByteOrder(data);
        return BitConverter.ToInt64(data);
    }

    public async Task WriteLong(long value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }

    public async Task<ulong> ReadULong()
    {
        var data = await ReadExactly(8);
        CorrectByteOrder(data);
        return BitConverter.ToUInt64(data);
    }

    public async Task WriteULong(ulong value)
    {
        var data = BitConverter.GetBytes(value);
        CorrectByteOrder(data);
        await Write(data);
    }
}

public class JavaServerStatusFetcher : IServerStatusFetcher
{
    private readonly string _host;
    private readonly int _port;

    public JavaServerStatusFetcher(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public async Task<MinecraftServerStatus> FetchStatus()
    {
        var connection = new JavaServerConnection(_host, _port);
    }
}