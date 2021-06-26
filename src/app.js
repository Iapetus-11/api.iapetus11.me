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

// load .env data
dotenv.config();

// add Minecraft font to canvas
canvas.registerFont("./src/assets/Minecraftia.ttf", {
  family: "Minecraft",
  style: "normal",
});

const app = express();

// add middleware
app.use(helmet());

// add routes
app.use("/mc/achievement", routeMinecraftAchievement);
app.use("/mc/splash", routeSplashScreen);
app.use("/mc/favicon", routeServerFavicon);
app.use("/mc/servercard", routeServerCard);
app.use("/mc/status", routeServerStatus);

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
