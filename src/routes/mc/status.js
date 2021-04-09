import express from "express";

import { mcStatus, parseAddress } from "../../util/mc/status.js";

const router = express.Router();

router.get("/:server", (req, res) => {
  let address;

  try {
    address = parseAddress(req.params.server);
  } catch (e) {
    res.status(400).json({
      success: false,
      message: "Bad Requests - URL parameter address is invalid.",
    });
    return;
  }

  mcStatus(...address)
    .then((status) => {
      res.status(200).json({ success: true, ...status });
    })
    .catch((e) => {
      console.log(e);
      res.status(500).json({
        success: false,
        message:
          "Error - An error occurred whilst checking the status of the server.",
      });
    });
});

export default router;
