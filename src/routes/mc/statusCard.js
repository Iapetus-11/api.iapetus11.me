import express from "express";
import canvas from "canvas";

import { drawImage, drawText, roundEdges, sendImage } from "../../util/canvas.js";
import { mcStatus, parseAddress } from "../../util/mc/status.js";
import { stringifyMotd } from "../../util/mc/motd.js";
import { minecraftColors, minecraftColorsCodes } from "../../minecraftFormatting.js";

const router = express.Router();

async function drawMotd(ctx, status) {
  let motd = status.motd;

  if (motd) {
    motd = stringifyMotd(motd).replace("Server.pro | ", "");
  } else if (!status.online) {
    motd = "This server is offline.";
  } else {
    motd = "A beautiful Minecraft server...";
  }

  ctx.font = '22px "Minecraft"';
  ctx.textAlign = "start";
  ctx.textBaseline = "bottom";
  ctx.fillStyle = "#".concat(minecraftColors.white.hex);

  let drawnPixels = 0;
  let drawnPixelsVerti = 0;
  let color;

  for (let i = 0; i < motd.length; i++) {
    if (motd.charAt(i) == "§") {
      // new color / formatting detected
      if (motd.charAt(i + 1)) {
        color = minecraftColorsCodes[motd.charAt(i + 1).toLowerCase()];
        if (color) ctx.fillStyle = "#".concat(color.hex);
      }

      i++; // skip over character that was used to set color
    } else {
      if (motd.charAt(i).indexOf("\n") != -1) {
        drawnPixelsVerti += 27;
        drawnPixels = 0;
      }

      ctx.fillText(motd.charAt(i), 146 + drawnPixels, 98 + drawnPixelsVerti);
      drawnPixels += ctx.measureText(motd.charAt(i)).width;
    }
  }
}

async function drawTopText(ctx, status, address, customName) {
  ctx.textBaseline = "bottom";
  ctx.textAlign = "start";

  const top = 25;

  let nameWidth = drawText(
    ctx,
    customName || address,
    146,
    top,
    "Minecraft",
    "#FFF",
    22,
    324,
    "start"
  );
  let playerWidth = drawText(
    ctx,
    `${status.players_online || 0}/${status.players_max || 0}`,
    762,
    top,
    "Minecraft",
    "#FFF",
    22,
    999,
    "end"
  );

  if (status.online) {
    // ctx.fillText(`${status.latency}ms`, ((146+nameWidth)+(762-playerWidth))/2, top);
    drawText(
      ctx,
      `${status.latency}ms`,
      (146 + nameWidth + (762 - playerWidth)) / 2,
      top,
      "Minecraft",
      "#FFF",
      22,
      324,
      "center"
    );
  }
}

router.get("/:server", async (req, res) => {
  let customName = req.query.name;
  let address;

  if (customName && 0 > customName.length > 30) {
    res.status(400).json({
      success: false,
      message: "Bad Request - Query parameter name is invalid",
    });
    return;
  }

  try {
    address = parseAddress(req.params.server);
  } catch (e) {
    res.status(400).json({
      success: false,
      message: "Bad Request - URL parameter server is invalid.",
    });
    return;
  }

  let status = await mcStatus(...address);

  let image = canvas.createCanvas(768, 140);
  let ctx = image.getContext("2d");

  roundEdges(ctx, 0, 0, 768, 140, 3); // make image corners rounded slightly

  // settings I think are best for dealing with pixely images
  ctx.imageSmoothingEnabled = false;
  ctx.quality = "nearest"; // nearest neighbor is best for dealing with pixels, it's Minecraft
  ctx.patternQuality = "nearest";
  ctx.textDrawingMode = "glyph";

  await drawImage(ctx, "./src/assets/dirt_background.png", 0, 0, 768, 140);

  let drawers = [
    drawMotd(ctx, status),
    drawImage(ctx, status.favicon || "./src/assets/unknown_pack.png", 6, 6, 128, 128),
    drawTopText(ctx, status, req.params.server, customName),
  ];

  await Promise.all(drawers);
  sendImage(image, res, "mcstatus.png");
});

export default router;
