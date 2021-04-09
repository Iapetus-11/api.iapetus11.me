import {bedrockServerStatus} from "./bedrock/status.js";

const validAddressChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:.";

export const defaultStatus = {
  "online": false,
  "players_online": 0,
  "players_max": 0,
  "players_names": [],
  "latency": 0,
  "version": {"brand": null, "software": null, "protocol": null},
  "motd": null,
  "favicon": null,
  "map": null,
  "gamemode": null
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
  return await bedrockServerStatus(host, port);
}
