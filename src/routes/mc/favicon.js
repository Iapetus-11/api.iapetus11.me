import express from "express";
import canvas from "canvas";

import { mcStatus, parseAddress } from "../../util/mc/status.js";
import { drawImage, sendImage } from "../../util/canvas.js";

const router = express.Router();

router.get("/:server", (req, res) => {
  let address;

  try {
    address = parseAddress(req.params.server);
  } catch (e) {
    res.status(400).json({
      success: false,
      message: "Bad Request - URL parameter server is invalid.",
    });
    return;
  }

  mcStatus(...address)
    .then((status) => {
      let image = canvas.createCanvas(64, 64);
      let ctx = image.getContext("2d");

      drawImage(
        ctx,
        status.favicon || "./src/assets/unknown_pack.png",
        0,
        0,
        64,
        64
      ).then(() => sendImage(image, res, "favicon.png"));
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
