import dns from "dns";

import { bedrockServerStatus } from "./bedrock.js";
import { javaServerStatus } from "./java.js";

const validAddressChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:.";
let statusCache = {}; // {server: {data}}

const clearStatusCacheLoop = () => {
  Object.keys(statusCache).forEach((key) => {
    if ((new Date() - statusCache[key].cache_time) / 1000 > 10) {
      delete statusCache[key];
    }
  });
};

setInterval(clearStatusCacheLoop, 1000);

export const defaultStatus = {
  online: false, // whether server is online or not
  latency: null, // latency in milliseconds between requesting status and getting response
  players_online: null, // number of players online
  players_max: null, // max number of players allowed
  players: [], // the names of the online players
  version: { brand: null, software: null, protocol: null }, // info about the server version and software
  motd: null, // the message of the day
  motd_raw: null, // the raw, unparsed message of the day straight from the server
  favicon: null, // the server icon / favicon that appears in the server list
  map: null, // the map the server is hosting
  gamemode: null, // the default gamemode the server is on
};

export const parseAddress = (address) => {
  // check address length
  if (4 >= address.length || address.length > 45) {
    throw new Error();
  }

  // ensure address contains a .
  if (address.split(".").length < 1) throw new Error();

  let colonCount = 0;

  // create colonCount and check allowed characters
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
    // check port for validness
    const split = address.split(":");
    const port = parseInt(split[1]);

    if (!port) {
      throw new Error();
    }

    return [split[0], port];
  } else {
    return [address, null];
  }
};

const getActualAddress = (host) => {
  return new Promise((resolve, reject) => {
    dns.resolveSrv(host, (e, addresses) => {
      try {
        resolve([addresses[0].name, addresses[1].port]);
      } catch (e) {
        reject();
      }
    });
  });
};

// fetches the status of the Minecraft server, retries once
const fetchMcStatus = async (host, port, doNotRetry) => {
  try {
    let actualAddress = await getActualAddress(host);
    host = actualAddress[0];
    port = actualAddress[1];
  } catch (e) {}

  try {
    return await Promise.any([
      bedrockServerStatus(host, port || 19132),
      javaServerStatus(host, port || 25565),
    ]);
  } catch (e) {
    if (!doNotRetry) {
      return await fetchMcStatus(host, port, true);
    }

    console.log(e);

    return defaultStatus;
  }
};

export const mcStatus = (host, port) => {
  return new Promise((resolve, reject) => {
    const cacheKey = [host, port];

    const cached = statusCache[cacheKey];

    if (cached) {
      resolve(cached);
    } else {
      fetchMcStatus(host, port)
        .then((status) => {
          status = { ...status, cached: false, cache_time: null };

          resolve(status);

          // insert into cache if the server was online
          if (status.online) {
            statusCache[cacheKey] = {
              ...status,
              cached: true,
              cache_time: new Date(),
            };
          }
        })
        .catch((e) => reject(e));
    }
  });
};
