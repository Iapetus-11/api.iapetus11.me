import net from "net";

class Connection {
  constructor(host, port) {
    this.host = host;
    this.port = port;

    this.sock = net.createConnection({host: host, port: port});

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
    process.nextTick(() => {
      try {
        return readRaw(n)
      } catch(e) {
        return read(n);
      }
    });
  }

  write(d) {
    return this.sock.write(d);
  }
}

export const javaServerStatus = (host, port) => {
  const con = new Connection(host, port);
};
