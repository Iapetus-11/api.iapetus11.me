import axios from "axios";

const imageFileEndings = [".png", ".jpg", ".gif", "jpeg"];

export const fetchSubredditImagePosts = (subreddits, limit) => {
  return new Promise((resolve, reject) => {
    let posts = [];

    axios.get(`https://reddit.com/r/${subreddits}/hot/.json?limit=${limit}`)
    .then(res => {
      res.data.data.children.forEach(post => {
        const p = post.data;

        if (!(p.removal_reason || p.is_video || p.pinned || p.stickied || p.selftext)) {
          if (p.url && imageFileEndings.includes(p.url.slice(-4))) {
            posts.push({
              id: p.id,
              subreddit: p.subreddit,
              author: p.author,
              title: p.title,
              permalink: 'https://reddit.com' + p.permalink,
              image: p.url,
              upvotes: p.ups,
              downvotes: p.downs,
              nsfw: p.over_18,
              spoiler: p.spoiler
            });
          }
        }
      });
    })
    .catch(e => reject(e));
  });
}