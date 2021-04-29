import { MUtf8Decoder, MUtf8Encoder } from "mutf-8";

const mutf8Decoder = new MUtf8Decoder();
const mutf8Encoder = new MUtf8Encoder();

const createServerData = (start, buf) => {
  const latency = new Date() - start;
  const data = mutf8Decoder.
}

const readVarInt = (buf, offset, maxBits) {
  maxBits = maxBits ? maxBits : 32;
  const numMin = -1 << (maxBits - 1);
  const numMax = 1 << (maxBits - 1);

  let num = 0;

  for (let i = 0; i < 10; i++) {
    let b = this.read(1, offset);
    offset += 1;
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

const createStatusHandshakePacket = (host, port) => {
  let buf = Buffer.from("");
}

export const javaServerStatus = (host, port) => {
  return new Promise((resolve, reject) => {
    const start = new Date();

    const sock = net.createConnection(port, host, () => {
      sock.write(Buffer.from([0xFE, 0x01]));
    });

    sock.on("error", (e) => reject(e));

    sock.on("data", (buf) => {
      sock.destroy();
      resolve(createServerData(start, buf));
    });
  });
};
