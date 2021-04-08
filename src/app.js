import express from "express";
import dotenv from "dotenv";

// load .env data
dotenv.config();

const app = express();

// run app
app.listen(process.env.PORT, () => {
  console.log(`Petu-API started on port ${process.env.PORT}`);
});
