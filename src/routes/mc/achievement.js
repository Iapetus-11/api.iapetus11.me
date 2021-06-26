import express from "express";
import canvas from "canvas";

import { drawImage, drawText, sendImage } from "../../util/canvas.js";

const router = express.Router();

router.get("/:text", async (req, res) => {
  let text = req.params.text;

  if (0 >= text.length > 30) {
    res
      .status(400)
      .json({ success: false, message: "Bad Request - URL parameter text is invalid" });
    return;
  }

  let image = canvas.createCanvas(320, 64);
  let ctx = image.getContext("2d");

  // We're dealing with intentionally pixely images so resizing should be nearest neighbor and smoothing should be off
  ctx.imageSmoothingEnabled = false;
  ctx.quality = "nearest";
  ctx.patternQuality = "nearest";
  ctx.textDrawingMode = "glyph";

  await drawImage(ctx, "./src/assets/achievement.png", 0, 0, 320, 64);
  drawText(ctx, text, 60, 45, "Minecraft", "#FFF", 15, 250, "left");

  sendImage(image, res, "achievement.png");
});

export default router;
