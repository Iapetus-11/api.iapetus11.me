import {bedrockServerStatus} from "./bedrock/status.js";

const validAddressChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:.";

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
    return address.split(":");
  } else {
    return [address, null];
  }
}

export const mcStatus = async (address) => {
  return await bedrockServerStatus()
}
