import net from "net";

const mcProtoVer = 754;

const writeVarInt = (buf, n, maxBits) => {
  maxBits = (maxBits ? maxBits : 32);
  const numMin = (-1 << (maxBits - 1));
  const numMax = (1 << (maxBits - 1));

  if (!(numMin <= num < numMax)) {
    throw new RangeError(`${num} doesn't fit in the range ${numMin} <= ${num} < ${numMax}`);
  }

  for (let i = 0; i < 10; i++) {
    let b = num & 0x7F;
    num = num >> 7;

    buf.writeUInt8(b | (num > 0 ? 0x80 : 0));

    if (num == 0) break;
  }
}

const readVarInt = (buf, n, maxBits) => {
  maxBits = (maxBits ? maxBits : 32);
  const numMin = (-1 << (maxBits - 1));
  const numMax = (1 << (maxBits - 1));

  let num = 0;

  for (let i = 0; i < 10; i++) {
    let b = buf.readUInt8();
    num = num | (b & 0x7F) << 7 * i;

    if (!(b & 0x80)) break;
  }

  if (num & (1 << 31)) num -= (1 << 32);

  if (!(numMin <= num < numMax)) {
    throw new RangeError(`${num} doesn't fit in the range ${numMin} <= ${num} < ${numMax}`);
  }

  return num;
}

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

  write(d) {
    return new Promise((resolve, reject) => {
      this.sock.write(d, () => {
        resolve();
      });
    });
  }
}

export const javaServerStatus = (host, port) => {
  const con = new Connection(host, port);
  buf.writeVarInt(mcProtoVer);
};
