import express from "express";

import {mcStatus, parseAddress} from "../../util/mc/common.js";

const router = express.Router();

router.get("/:server", (req, res) => {
  let address;

  try {
    address = parseAddress(req.params.server);
  } catch (e) {
    res.status(400).json({success: false, message: "Bad Requests - URL parameter address is invalid."});
    return;
  }

  mcStatus(...address);

  res.status(200).json({success: true, message: address});
});

export default router;
