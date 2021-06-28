import express from "express";
import { fetchSubredditImagePosts } from "../../util/reddit.js";

const router = express.Router();

const subreddits = "comics";
let lastPosts = {};
let postList = [];

const updateMemes = () =>
  fetchSubredditImagePosts(subreddits, 500).then((posts) => (postList = posts));

// populate the memes initially
fetchSubredditImagePosts(subreddits, 25).then((posts) => (postList = posts));
fetchSubredditImagePosts(subreddits, 500).then((posts) => (postList = posts));

// update memes every 10 min
setInterval(updateMemes, 1000 * 60 * 10);

router.get("/", (req, res) => {
  const queryId = (req.query.queryId || "").slice(0, 24);

  if (queryId) {
    // we need to get a meme that has not been shown for this queryId for the last 7 times
    let lastQueryPosts = lastPosts[queryId];

    if (lastQueryPosts) {
      let post, postId;
      let tries = 0;

      while ((!memeId || lastQueryPosts.has(postId)) && tries < 7) {
        post = postList[Math.floor(Math.random() * postList.length)];
        postId = post.id;
        tries++;
      }

      lastQueryPosts.add(postId);

      if (lastQueryPosts.size >= 9) {
        let i = 0;

        [...lastQueryPosts].reverse().forEach((post) => {
          if (i > 9) lastQueryPosts.delete(post);
          i++;
        });
      }

      res.status(200).json(post);
      return;
    } else {
      const post = postList[Math.floor(Math.random() * postList.length)];
      lastPosts[queryId] = new Set([post.id]);

      res.status(200).json(post);
      return;
    }
  }

  res.status(200).json(postList[Math.floor(Math.random() * postList.length)]);
});

export default router;
