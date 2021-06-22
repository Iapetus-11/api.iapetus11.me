import canvas from "canvas";

// function for drawing an image easily onto the ctx
export function drawImage(ctx, src, x, y, width, height) {
  return new Promise((resolve, reject) => {
    let image = new canvas.Image();

    image.onload = function () {
      ctx.drawImage(image, x, y, width, height);
      resolve();
    };

    image.src = src;
  });
}

// function for drawing text that auto resizes easily
export function drawText(
  ctx,
  text,
  x,
  y,
  fontName,
  color,
  defaultSize,
  maxWidth,
  alignment
) {
  ctx.textBaseline = "middle";
  ctx.fillStyle = color;
  ctx.textAlign = alignment;

  ctx.font = `${defaultSize}px "${fontName}"`;

  // ensmallen the text till it fits
  while (ctx.measureText(text).width > maxWidth) {
    defaultSize -= 0.1;
    ctx.font = `${defaultSize}px "${fontName}"`;
  }

  ctx.fillText(text, x, y);

  return ctx.measureText(text).width;
}

// make a rectangular clip / border with round edges on the given ctx
export function roundEdges(ctx, x, y, width, height, radius) {
  // definitely didn't steal this from code I did on disbots.gg hehe
  ctx.beginPath();
  ctx.moveTo(x + radius, y);
  ctx.lineTo(x + width - radius, y);
  ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
  ctx.lineTo(x + width, y + height - radius);
  ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
  ctx.lineTo(x + radius, y + height);
  ctx.quadraticCurveTo(x, y + height, x, y + height - radius);
  ctx.lineTo(x, y + radius);
  ctx.quadraticCurveTo(x, y, x + radius, y);
  ctx.closePath();
  ctx.clip();
}

// function to send an image easily
export function sendImage(image, res, fileName) {
  image.toBuffer((e, buffer) => {
    // this will send the image straight from the buffer without saving first
    res
      .writeHead(200, {
        "Content-Type": "image/png",
        "Content-Disposition": `inline;filename=${fileName}`, // inline or attachment
        "Content-Length": buffer.length,
      })
      .end(Buffer.from(buffer, "binary"));
  });
}
