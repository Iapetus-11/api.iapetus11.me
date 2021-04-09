import {bedrockServerStatus} from "./bedrock/status.js";

const validAddressChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:.";

export const defaultStatus = {
  "online": false, // whether server is online or not
  "latency": null,  // latency in milliseconds between requesting status and getting response
  "players_online": null, // number of players online
  "players_max": null, // max number of players allowed
  "players_names": [], // the names of the online players
  "version": {"brand": null, "software": null, "protocol": null}, // info about the server version and software
  "motd": null, // the message of the day
  "favicon": null, // the server icon / favicon that appears in the server list
  "map": null, // the map the server is hosting
  "gamemode": null // the default gamemode the server is on
};

export const parseAddress = (address) => {
  if (4 >= address.length > 30) {
    throw new Error();
  }

  let colonCount = 0;

  for (let c of address) {
    if (c === ":") {
      colonCount += 1;
    }

    if (!validAddressChars.includes(c)) {
      throw new Error();
    }
  }

  if (colonCount > 1) {
    throw new Error();
  } else if (colonCount === 1) {
    const split = address.split(":");
    const port = parseInt(split[1]);

    if (!port) {
      throw new Error();
    }

    return [split[0], port];
  } else {
    return [address, null];
  }
}

export const mcStatus = async (host, port) => {
  try {
    return await Promise.any([bedrockServerStatus(host, port)]);
  } catch (e) {
    return defaultStatus;
  }
}
