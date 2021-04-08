import express from "express";
import dotenv from "dotenv";
import helmet from "helmet";

// load .env data
dotenv.config();

const app = express();

// use middleware
app.use(helmet());

// run app
app.listen(process.env.PORT, () => {
  console.log(`Petu-API started on port ${process.env.PORT}`);
});
