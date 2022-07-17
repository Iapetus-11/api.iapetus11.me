using System.Text.Json.Serialization;

namespace api.iapetus11.me.Models.GitHub;

using System;

public record Repository
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = null!;

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    // [JsonPropertyName("owner")]
    // public Owner Owner { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("fork")]
    public bool Fork { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("forks_url")]
    public Uri ForksUrl { get; set; } = null!;

    [JsonPropertyName("keys_url")]
    public string KeysUrl { get; set; } = null!;

    [JsonPropertyName("collaborators_url")]
    public string CollaboratorsUrl { get; set; } = null!;

    [JsonPropertyName("teams_url")]
    public Uri TeamsUrl { get; set; } = null!;

    [JsonPropertyName("hooks_url")]
    public Uri HooksUrl { get; set; } = null!;

    [JsonPropertyName("issue_events_url")]
    public string IssueEventsUrl { get; set; } = null!;

    [JsonPropertyName("events_url")]
    public Uri EventsUrl { get; set; } = null!;

    [JsonPropertyName("assignees_url")]
    public string AssigneesUrl { get; set; } = null!;

    [JsonPropertyName("branches_url")]
    public string BranchesUrl { get; set; } = null!;

    [JsonPropertyName("tags_url")]
    public Uri TagsUrl { get; set; } = null!;

    [JsonPropertyName("blobs_url")]
    public string BlobsUrl { get; set; } = null!;

    [JsonPropertyName("git_tags_url")]
    public string GitTagsUrl { get; set; } = null!;

    [JsonPropertyName("git_refs_url")]
    public string GitRefsUrl { get; set; } = null!;

    [JsonPropertyName("trees_url")]
    public string TreesUrl { get; set; } = null!;

    [JsonPropertyName("statuses_url")]
    public string StatusesUrl { get; set; } = null!;

    [JsonPropertyName("languages_url")]
    public Uri LanguagesUrl { get; set; } = null!;

    [JsonPropertyName("stargazers_url")]
    public Uri StargazersUrl { get; set; } = null!;

    [JsonPropertyName("contributors_url")]
    public Uri ContributorsUrl { get; set; } = null!;

    [JsonPropertyName("subscribers_url")]
    public Uri SubscribersUrl { get; set; } = null!;

    [JsonPropertyName("subscription_url")]
    public Uri SubscriptionUrl { get; set; } = null!;

    [JsonPropertyName("commits_url")]
    public string CommitsUrl { get; set; } = null!;

    [JsonPropertyName("git_commits_url")]
    public string GitCommitsUrl { get; set; } = null!;

    [JsonPropertyName("comments_url")]
    public string CommentsUrl { get; set; } = null!;

    [JsonPropertyName("issue_comment_url")]
    public string IssueCommentUrl { get; set; } = null!;

    [JsonPropertyName("contents_url")]
    public string ContentsUrl { get; set; } = null!;

    [JsonPropertyName("compare_url")]
    public string CompareUrl { get; set; } = null!;

    [JsonPropertyName("merges_url")]
    public Uri MergesUrl { get; set; } = null!;

    [JsonPropertyName("archive_url")]
    public string ArchiveUrl { get; set; } = null!;

    [JsonPropertyName("downloads_url")]
    public Uri DownloadsUrl { get; set; } = null!;

    [JsonPropertyName("issues_url")]
    public string IssuesUrl { get; set; } = null!;

    [JsonPropertyName("pulls_url")]
    public string PullsUrl { get; set; } = null!;

    [JsonPropertyName("milestones_url")]
    public string MilestonesUrl { get; set; } = null!;

    [JsonPropertyName("notifications_url")]
    public string NotificationsUrl { get; set; } = null!;

    [JsonPropertyName("labels_url")]
    public string LabelsUrl { get; set; } = null!;

    [JsonPropertyName("releases_url")]
    public string ReleasesUrl { get; set; } = null!;

    [JsonPropertyName("deployments_url")]
    public Uri DeploymentsUrl { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("pushed_at")]
    public DateTimeOffset PushedAt { get; set; }

    [JsonPropertyName("git_url")]
    public string GitUrl { get; set; } = null!;

    [JsonPropertyName("ssh_url")]
    public string SshUrl { get; set; } = null!;

    [JsonPropertyName("clone_url")]
    public Uri CloneUrl { get; set; } = null!;

    [JsonPropertyName("svn_url")]
    public Uri SvnUrl { get; set; } = null!;

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; } = null!;

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("stargazers_count")]
    public long StargazersCount { get; set; }

    [JsonPropertyName("watchers_count")]
    public long WatchersCount { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; } = null!;

    [JsonPropertyName("has_issues")]
    public bool HasIssues { get; set; }

    [JsonPropertyName("has_projects")]
    public bool HasProjects { get; set; }

    [JsonPropertyName("has_downloads")]
    public bool HasDownloads { get; set; }

    [JsonPropertyName("has_wiki")]
    public bool HasWiki { get; set; }

    [JsonPropertyName("has_pages")]
    public bool HasPages { get; set; }

    [JsonPropertyName("forks_count")]
    public long ForksCount { get; set; }

    [JsonPropertyName("mirror_url")]
    public object MirrorUrl { get; set; } = null!;

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }

    [JsonPropertyName("open_issues_count")]
    public long OpenIssuesCount { get; set; }

    // [JsonPropertyName("license")]
    // public License License { get; set; } = null!;

    [JsonPropertyName("allow_forking")]
    public bool AllowForking { get; set; }

    [JsonPropertyName("is_template")]
    public bool IsTemplate { get; set; }

    [JsonPropertyName("web_commit_signoff_required")]
    public bool WebCommitSignoffRequired { get; set; }

    [JsonPropertyName("topics")]
    public string[] Topics { get; set; } = null!;

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; } = null!;

    [JsonPropertyName("forks")]
    public long Forks { get; set; }

    [JsonPropertyName("open_issues")]
    public long OpenIssues { get; set; }

    [JsonPropertyName("watchers")]
    public long Watchers { get; set; }

    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; } = null!;
}

public record License
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("spdx_id")]
    public string SpdxId { get; set; } = null!;

    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;
}

public record Owner
{
    [JsonPropertyName("login")] public string Login { get; set; } = null!;

    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("node_id")] public string NodeId { get; set; } = null!;

    [JsonPropertyName("avatar_url")] public Uri AvatarUrl { get; set; } = null!;

    [JsonPropertyName("gravatar_id")] public string GravatarId { get; set; } = null!;

    [JsonPropertyName("url")] public Uri Url { get; set; } = null!;

    [JsonPropertyName("html_url")] public Uri HtmlUrl { get; set; } = null!;

    [JsonPropertyName("followers_url")] public Uri FollowersUrl { get; set; } = null!;

    [JsonPropertyName("following_url")] public string FollowingUrl { get; set; } = null!;

    [JsonPropertyName("gists_url")] public string GistsUrl { get; set; } = null!;

    [JsonPropertyName("starred_url")] public string StarredUrl { get; set; } = null!;

    [JsonPropertyName("subscriptions_url")] public Uri SubscriptionsUrl { get; set; } = null!;

    [JsonPropertyName("organizations_url")] public Uri OrganizationsUrl { get; set; } = null!;

    [JsonPropertyName("repos_url")] public Uri ReposUrl { get; set; } = null!;

    [JsonPropertyName("events_url")] public string EventsUrl { get; set; } = null!;

    [JsonPropertyName("received_events_url")] public Uri ReceivedEventsUrl { get; set; } = null!;

    [JsonPropertyName("type")] public string Type { get; set; } = null!;

    [JsonPropertyName("site_admin")] public bool SiteAdmin { get; set; }
}
