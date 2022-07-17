using System.Text.Json.Serialization;

namespace api.iapetus11.me.Models.GitHub;

using System;

public record SearchResult
{
    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }

    [JsonPropertyName("incomplete_results")]
    public bool IncompleteResults { get; set; }

    [JsonPropertyName("items")]
    public SearchItem[] Items { get; set; } = null!;
}

public record SearchItem
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("repository_url")]
    public Uri RepositoryUrl { get; set; } = null!;

    [JsonPropertyName("labels_url")]
    public string LabelsUrl { get; set; } = null!;

    [JsonPropertyName("comments_url")]
    public Uri CommentsUrl { get; set; } = null!;

    [JsonPropertyName("events_url")]
    public Uri EventsUrl { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;

    [JsonPropertyName("number")]
    public long Number { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    // [JsonPropertyName("user")]
    // public User User { get; set; } = null!;

    // [JsonPropertyName("labels")]
    // public Label[] Labels { get; set; } = null!;

    [JsonPropertyName("state")]
    public string State { get; set; } = null!;

    [JsonPropertyName("locked")]
    public bool Locked { get; set; }

    // [JsonPropertyName("assignee")]
    // public User Assignee { get; set; } = null!;

    // [JsonPropertyName("assignees")]
    // public User[] Assignees { get; set; } = null!;

    // [JsonPropertyName("milestone")]
    // public Milestone Milestone { get; set; } = null!;

    [JsonPropertyName("comments")]
    public long Comments { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTimeOffset? ClosedAt { get; set; }

    [JsonPropertyName("author_association")]
    public string AuthorAssociation { get; set; } = null!;

    // [JsonPropertyName("draft")]
    // public bool? Draft { get; set; }

    // [JsonPropertyName("pull_request", NullValueHandling = NullValueHandling.Ignore)]
    // public PullRequest PullRequest { get; set; } = null!;

    [JsonPropertyName("body")]
    public string Body { get; set; } = null!;

    [JsonPropertyName("reactions")]
    public Reactions Reactions { get; set; } = null!;

    [JsonPropertyName("timeline_url")]
    public Uri TimelineUrl { get; set; } = null!;

    [JsonPropertyName("state_reason")]
    public string StateReason { get; set; } = null!;

    [JsonPropertyName("score")]
    public double Score { get; set; }
}

public record User
{
    [JsonPropertyName("login")]
    public string Login { get; set; } = null!;

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;

    [JsonPropertyName("avatar_url")]
    public Uri AvatarUrl { get; set; } = null!;

    [JsonPropertyName("gravatar_id")]
    public string GravatarId { get; set; } = null!;

    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("followers_url")]
    public Uri FollowersUrl { get; set; } = null!;

    [JsonPropertyName("following_url")]
    public string FollowingUrl { get; set; } = null!;

    [JsonPropertyName("gists_url")]
    public string GistsUrl { get; set; } = null!;

    [JsonPropertyName("starred_url")]
    public string StarredUrl { get; set; } = null!;

    [JsonPropertyName("subscriptions_url")]
    public Uri SubscriptionsUrl { get; set; } = null!;

    [JsonPropertyName("organizations_url")]
    public Uri OrganizationsUrl { get; set; } = null!;

    [JsonPropertyName("repos_url")]
    public Uri ReposUrl { get; set; } = null!;

    [JsonPropertyName("events_url")]
    public string EventsUrl { get; set; } = null!;

    [JsonPropertyName("received_events_url")]
    public Uri ReceivedEventsUrl { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("site_admin")]
    public bool SiteAdmin { get; set; }
}

public record Label
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;

    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("color")]
    public string Color { get; set; } = null!;

    [JsonPropertyName("default")]
    public bool Default { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
}

public record Milestone
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("labels_url")]
    public Uri LabelsUrl { get; set; } = null!;

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;

    [JsonPropertyName("number")]
    public long Number { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("creator")]
    public User Creator { get; set; } = null!;

    [JsonPropertyName("open_issues")]
    public long OpenIssues { get; set; }

    [JsonPropertyName("closed_issues")]
    public long ClosedIssues { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("due_on")]
    public DateTimeOffset? DueOn { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTimeOffset? ClosedAt { get; set; }
}

public class PullRequest
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("diff_url")]
    public Uri DiffUrl { get; set; } = null!;

    [JsonPropertyName("patch_url")]
    public Uri PatchUrl { get; set; } = null!;

    [JsonPropertyName("merged_at")]
    public DateTimeOffset? MergedAt { get; set; }
}

public record Reactions
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }

    [JsonPropertyName("+1")]
    public long The1 { get; set; }

    [JsonPropertyName("-1")]
    public long Reactions1 { get; set; }

    [JsonPropertyName("laugh")]
    public long Laugh { get; set; }

    [JsonPropertyName("hooray")]
    public long Hooray { get; set; }

    [JsonPropertyName("confused")]
    public long Confused { get; set; }

    [JsonPropertyName("heart")]
    public long Heart { get; set; }

    [JsonPropertyName("rocket")]
    public long Rocket { get; set; }

    [JsonPropertyName("eyes")]
    public long Eyes { get; set; }
}
