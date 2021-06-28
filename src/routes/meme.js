import express, { query } from "express";
import {fetchSubredditImagePosts} from "../util/reddit.js";

const router = express.Router();

const memeSubreddits = "memes+me_irl+dankmemes+wholesomememes+prequelmemes+comedyheaven";
let lastMemes = {};
let memes = [];

const updateMemes = () => fetchSubredditImagePosts(memeSubreddits, 500).then(posts => memes = posts);

// populate the memes initially
fetchSubredditImagePosts(memeSubreddits, 25).then(posts => memes = posts);

// update memes every 10 min
setInterval(updateMemes, 1000 * 60 * 10);

router.get("/", (req, res) => {
  const queryId = req.query.queryId.slice(0, 24);

  if (queryId) { // we need to get a meme that has not been shown for this queryId for the last 7 times
    let lastQueryMemes = lastMemes[queryId];

    if (lastQueryMemes) {
      let meme, memeId;

      while (!memeId || lastQueryMemes.includes(memeId)) {
        meme = memes[Math.floor(Math.random() * memes.length)];
        memeId = meme.id;
      }

      lastQueryMemes.push(memeId);

      if (lastQueryMemes.length > 7) lastQueryMemes.shift();

      res.status(200).json(meme);
      return;
    } else {
      const meme = memes[Math.floor(Math.random() * memes.length)];
      lastMemes[queryId] = [meme.id];

      res.status(200).json(meme);
      return;
    }
  }

  res.status(200).json(memes[Math.floor(Math.random() * memes.length)]);
});

export default router;