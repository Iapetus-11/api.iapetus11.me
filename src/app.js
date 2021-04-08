import express from "express";
import dotenv from "dotenv";
import helmet from "helmet";

// load .env data
dotenv.config();

const app = express();

// use middleware
app.use(helmet());

app.use((req, res) => { // handle 404s, must be after middleware and routes to work
  res.status(404).json({success: false, message: 'Error - Endpoint not found or method not supported for this endpoint.'});
});

// run app
app.listen(process.env.PORT, () => {
  console.log(`Petu-API started on port ${process.env.PORT}`);
});
