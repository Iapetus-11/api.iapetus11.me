import { Client, PacketWriter, State } from "mcproto";

export const javaServerStatus = async (host, port) => {
  const client = await Client.connect(host, port, {
    connectTimeout: 1000,
    timeout: 1000,
  });

  // send handshake and get basic status
  client.send(
    new PacketWriter(0x0)
      .writeVarInt(755) // 1.17 protocol version
      .writeString(host)
      .writeUInt16(port)
      .writeVarInt(State.Status)
  );
  client.send(new PacketWriter(0x0));
  const status = (await client.nextPacket()).readJSON();

  // get latency / ping as it would appear in the server list
  client.send(new PacketWriter(0x1).write(Buffer.alloc(8)));
  const start = Date.now();

  await client.nextPacket(0x1);
  const latency = Date.now() - start;

  client.end();

  return {
    online: true,
    latency: latency,
    players_online: status.players.online,
    players_max: status.players.max,
    players: status.players.sample || [],
    version: {
      brand: "Java Edition",
      software: status.version.name,
      protocol: status.version.protocol,
    },
    motd: status.description,
    favicon: status.favicon,
    map: null,
    gamemode: null,
  };
};
