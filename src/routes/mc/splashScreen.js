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

  let image = canvas.createCanvas(512, 271);
  let ctx = image.getContext("2d");

  // We're dealing with inentionally pixely images so resizing should be nearest neighbor and smoothing should be off
  ctx.imageSmoothingEnabled = false;
  ctx.quality = "nearest";
  ctx.patternQuality = "nearest";
  ctx.textDrawingMode = "glyph";

  await drawImage(ctx, "./src/assets/splash.png", 0, 0, 512, 271);

  ctx.rotate(-0.45);

  drawText(ctx, text, 306, 220, "Minecraft", "#475C11", 12, 400, "center");
  drawText(ctx, text, 305, 219, "Minecraft", "#FFFF55", 12, 400, "center");

  sendImage(image, res, "splash.png");
});

export default router;
