import { stringifyMotd } from "./motd.js";
import dgram from "dgram";

const handshake = Buffer.from("01000000000000000000ffff00fefefefefdfdfdfd12345678", "hex");

export const bedrockServerStatus = (host, port) => {
  return new Promise((resolve, reject) => {
    const socket = dgram.createSocket("udp4");
    let start;

    socket.on("error", (e) => {
      reject(e);
    });

    socket.on("message", (data, info) => {
      var status;

      try {
        socket.close();

        let status_length = data.readUint16BE(33);
        let status_data = data
          .slice(35, 35 + status_length)
          .toString()
          .split(";");

        status = {
          online: true,
          latency: new Date() - start,
          players_online: parseInt(status_data[4]),
          players_max: parseInt(status_data[5]),
          players: [],
          version: {
            brand: status_data[0],
            software: `Bedrock ${status_data[3]}`,
            protocol: status_data[2],
          },
          motd: stringifyMotd(status_data[1]),
          motd_raw: status_data[1],
          favicon: null,
        };
      } catch (e) {
        reject(e);
      }

      try {
        status.map = status_data[7];
        status.gamemode = status_data[8];
      } catch (e) {}

      resolve(status);
    });

    socket.send(handshake, 0, handshake.length, port, host, (e) => {
      start = new Date();

      if (e) {
        reject(e);
        try {
          socket.close();
        } catch (e) {}
      }
    });

    setTimeout(() => {
      try {
        socket.close();
      } catch (e) {}
      reject();
    }, 1000);
  });
};
