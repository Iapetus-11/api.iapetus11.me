import rateLimit from "express-rate-limit";
import express from "express";
import canvas from "canvas";
import dotenv from "dotenv";
import helmet from "helmet";

// import routes
import routeMinecraftAchievement from "./routes/mc/achievement.js";
import routeSplashScreen from "./routes/mc/splashScreen.js";
import routeServerFavicon from "./routes/mc/favicon.js";
import routeServerCard from "./routes/mc/statusCard.js";
import routeServerStatus from "./routes/mc/status.js";

import routeGimmeCursedMinecraft from "./routes/reddit/cursedMinecraft.js";
import routeGimmeGreentext from "./routes/reddit/greentext.js";
import routeGimmeComic from "./routes/reddit/comic.js";
import routeGimmeMeme from "./routes/reddit/meme.js";

// load .env data
dotenv.config();

// add Minecraft font to canvas
canvas.registerFont("./src/assets/Minecraftia.ttf", {
  family: "Minecraft",
  style: "normal",
});

const app = express();

const rateLimitKeyGenerator = (req) => req.get("CF-Connecting-IP") || req.ip;
const rateLimitSkipHandler = (req, res) => process.env.BYPASS == req.get("Authorization");
const rateLimitHandler = (req, res) => {
  return res.status(429).json({
    message: "Error - You've hit the rate limit",
    limit: req.rateLimit.limit,
    current: req.rateLimit.current,
    remaining: req.rateLimit.remaining,
  });
};

// per is in seconds, so how many requests (limit) per second (per)
const createRateLimit = (limit, per) => {
  return rateLimit({
    windowMs: per * 1000,
    max: limit,
    keyGenerator: rateLimitKeyGenerator,
    skip: rateLimitSkipHandler,
    handler: rateLimitHandler,
  });
};

// add middleware
app.use(helmet());

// add routes
app.use("/mc/achievement", createRateLimit(4, 1), routeMinecraftAchievement);
app.use("/mc/splash", createRateLimit(5, 1), routeSplashScreen);
app.use("/mc/favicon", createRateLimit(5, 1), routeServerFavicon);
app.use("/mc/servercard", createRateLimit(5, 1), routeServerCard);
app.use("/mc/status", createRateLimit(5, 1), routeServerStatus);

app.use("/reddit/cursedminecraft", createRateLimit(10, 1), routeGimmeCursedMinecraft);
app.use("/reddit/greentext", createRateLimit(10, 1), routeGimmeGreentext);
app.use("/reddit/comic", createRateLimit(10, 1), routeGimmeComic);
app.use("/reddit/meme", createRateLimit(10, 1), routeGimmeMeme);

// handle 404s, must be after middleware and routes to work
app.use((req, res) => {
  res.status(404).json({
    success: false,
    message: "Error - Endpoint not found or method not supported for this endpoint.",
  });
});

// run app
app.listen(process.env.PORT || 42069, () => {
  console.log(`Petu-API started on port ${process.env.PORT || 42069}`);
});
