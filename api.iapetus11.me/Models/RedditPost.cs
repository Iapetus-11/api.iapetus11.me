using System.Text.Json.Serialization;

namespace api.iapetus11.me.Models;

public record RedditPost(string Id, string Subreddit, string Author, string Title, string Permalink,
    [property: JsonPropertyName("image")] string ImageUrl, int Upvotes, int Downvotes, bool Nsfw,
    bool Spoiler);