import net from "net";

const mcProtoVer = 754;

class Connection {
  constructor(host, port) {
    this.host = host;
    this.port = port;

    this.sock = net.createConnection({ host: host, port: port });

    this.buffer = new Buffer();
    this.pos = 0;

    this.sock.on("data", (data) => {
      this.buffer = Buffer.concat([this.buffer, Buffer.from(data)]);
    });
  }

  readRaw(n) {
    if (this.buffer.length >= this.pos + n) {
      this.pos += n;
      return this.buffer.slice(this.pos - n, this.pos);
    } else {
      throw new Error();
    }
  }

  read(n) {
    setImmediate(() => {
      try {
        return readRaw(n);
      } catch (e) {
        return read(n);
      }
    });
  }

  readVarInt(maxBits) {
    maxBits = maxBits ? maxBits : 32;
    const numMin = -1 << (maxBits - 1);
    const numMax = 1 << (maxBits - 1);

    let num = 0;

    for (let i = 0; i < 10; i++) {
      let b = this.read(1);
      this.pos += 1;
      num = num | ((b & 0x7f) << (7 * i));

      if (!(b & 0x80)) break;
    }

    if (num & (1 << 31)) num -= 1 << 32;

    if (!(numMin <= num < numMax)) {
      throw new RangeError(
        `${num} doesn't fit in the range ${numMin} <= ${num} < ${numMax}`
      );
    }

    return num;
  }

  writeVarInt(n, maxBits) {
    return new Promise((resolve, reject) => {
      maxBits = maxBits ? maxBits : 32;
      const numMin = -1 << (maxBits - 1);
      const numMax = 1 << (maxBits - 1);

      if (!(numMin <= num < numMax)) {
        throw new RangeError(
          `${num} doesn't fit in the range ${numMin} <= ${num} < ${numMax}`
        );
      }

      let buf = new Buffer();

      for (let i = 0; i < 10; i++) {
        let b = num & 0x7f;
        num = num >> 7;

        buf.writeUInt8(b | (num > 0 ? 0x80 : 0));

        if (num == 0) break;
      }

      this.sock.write(buf).then((flushed) => resolve()).catch((e) => reject(e));
    });
  }
}

export const javaServerStatus = (host, port) => {
  const con = new Connection(host, port);
  let buf = new Buffer();

  writePacketHandshake(buf, 754, host, port, 1);

  let upcomingLen =
};
