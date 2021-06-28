import express from "express";
import {fetchSubredditImagePosts} from "../util/reddit.js";

const router = express.Router();

const memeSubreddits = "memes+me_irl+dankmemes+wholesomememes+prequelmemes+comedyheaven";
let memes = new Set();

const updateMemes = () => fetchSubredditImagePosts(memeSubreddits, 500).then(posts => memes = new Set(posts));

// populate the memes initially
updateMemes();

// update memes every 10 min
setInterval(updateMemes, 1000 * 60 * 10);

router.get("/", (req, res) => {
  res.status(200).json(memes[Math.floor(Math.random() * memes.length)]);
});

export default router;