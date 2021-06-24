import express from "express";
import dotenv from "dotenv";
import helmet from "helmet";

// import routes
import routeServerFavicon from "./routes/mc/favicon.js";
import routeMCStatus from "./routes/mc/status.js";

// load .env data
dotenv.config();

const app = express();

// use middleware
app.use(helmet());

// add routes
app.use("/mc/status", routeMCStatus);
app.use("/mc/favicon", routeServerFavicon);

// handle 404s, must be after middleware and routes to work
app.use((req, res) => {
  res.status(404).json({
    success: false,
    message:
      "Error - Endpoint not found or method not supported for this endpoint.",
  });
});

// run app
app.listen(process.env.PORT || 42069, () => {
  console.log(`Petu-API started on port ${process.env.PORT || 42069}`);
});
