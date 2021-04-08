import express from "express";

const router = express.Router();

router.get("/:address", (req, res) => {
  const address = req.params.address;

  if (address.length <= 4) {
    res.status(400).json({success: false, message: "Bad Request - URL parameter address is invalid."});
    return;
  }

  res.status(200).json({success: true, message: "bruh"});
});

export default router;
