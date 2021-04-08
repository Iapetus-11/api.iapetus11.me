
const validAddressChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:.";

export const validateAddress = (address) => {
  if (4 >= address.length > 30) {
    throw new Error();
  }

  for (let c of address) {
    if (!validAddressChars.includes(c)) {
      throw new Error();
    }
  }
}

export const mcStatus = (address) => {
  throw new Error("Not implemented yet...");
}
