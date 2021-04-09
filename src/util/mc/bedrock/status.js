import buffer from "buffer";
import udp from "udp";

const handshake = Buffer.from("\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\xff\xff\x00\xfe\xfe\xfe\xfe\xfd\xfd\xfd\xfd\x124Vx");

export const bedrockServerStatus = (host, port) => {
  return new Promise((resolve, reject) => {
    const socket = udp.createSocket("udp4");

    socket.connect(port, host, () => {
      socket.send(handshake, port, host, (e) => {
        if (e) {
          reject(e);
        } else {
          socket.on("message", (msg, info) => {
            console.log(msg);
          });
        }
      });
    });
  });
}
