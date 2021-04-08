import express from "express";

import {mcStatus, validateAddress} from "../../util/mc/common.js";

const router = express.Router();

router.get("/:address", (req, res) => {
  const address = req.params.address;

  try {
    validateAddress(address);
  } catch (e) {
    res.status(400).json({success: false, message: "Bad Requests - URL parameter address is invalid."});
    return;
  }

  res.status(200).json({success: true, message: "bruh"});
});

export default router;
