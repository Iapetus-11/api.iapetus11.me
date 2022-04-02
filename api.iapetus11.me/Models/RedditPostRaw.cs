#pragma warning disable CS8618

using Newtonsoft.Json;

// auto generated models
// ReSharper disable ClassNeverInstantiated.Global

namespace api.iapetus11.me.Models;

using J = JsonPropertyAttribute;
using R = Required;
using N = NullValueHandling;

public class RedditListing
{
    [J("kind")] public string Kind { get; set; }
    [J("data")] public RedditListingData Data { get; set; }
}

public class RedditListingData
{
    [J("after")] public string After { get; set; }
    [J("dist")] public long Dist { get; set; }
    [J("modhash")] public string Modhash { get; set; }
    [J("geo_filter")] public object GeoFilter { get; set; }
    [J("children")] public RedditPostRaw[] Children { get; set; }
    [J("before")] public object Before { get; set; }
}

public class RedditPostRaw
{
    [J("kind")] public string Kind { get; set; }
    [J("data")] public RedditPostRawData Data { get; set; }
}

public class RedditPostRawData
{
    [J("approved_at_utc")] public object ApprovedAtUtc { get; set; }
    [J("subreddit")] public string Subreddit { get; set; }
    [J("selftext")] public string Selftext { get; set; }
    [J("author_fullname")] public string AuthorFullname { get; set; }
    [J("saved")] public bool Saved { get; set; }
    [J("mod_reason_title")] public object ModReasonTitle { get; set; }
    [J("gilded")] public long Gilded { get; set; }
    [J("clicked")] public bool Clicked { get; set; }
    [J("title")] public string Title { get; set; }
    [J("link_flair_richtext")] public object[] LinkFlairRichtext { get; set; }
    [J("subreddit_name_prefixed")] public string SubredditNamePrefixed { get; set; }
    [J("hidden")] public bool Hidden { get; set; }
    [J("pwls")] public long? Pwls { get; set; }
    [J("link_flair_css_class")] public object LinkFlairCssClass { get; set; }
    [J("downs")] public long Downs { get; set; }
    [J("thumbnail_height")] public long? ThumbnailHeight { get; set; }
    [J("top_awarded_type")] public string TopAwardedType { get; set; }
    [J("hide_score")] public bool HideScore { get; set; }
    [J("name")] public string Name { get; set; }
    [J("quarantine")] public bool Quarantine { get; set; }
    [J("link_flair_text_color")] public string LinkFlairTextColor { get; set; }
    [J("upvote_ratio")] public double UpvoteRatio { get; set; }
    [J("author_flair_background_color")] public string AuthorFlairBackgroundColor { get; set; }
    [J("subreddit_type")] public string SubredditType { get; set; }
    [J("ups")] public long Ups { get; set; }
    [J("total_awards_received")] public long TotalAwardsReceived { get; set; }
    [J("media_embed")] public RedditMediaEmbed RedditMediaEmbed { get; set; }
    [J("thumbnail_width")] public long? ThumbnailWidth { get; set; }
    [J("author_flair_template_id")] public string AuthorFlairTemplateId { get; set; }
    [J("is_original_content")] public bool IsOriginalContent { get; set; }
    [J("user_reports")] public object[] UserReports { get; set; }
    [J("secure_media")] public RedditMedia SecureRedditMedia { get; set; }
    [J("is_reddit_media_domain")] public bool IsRedditMediaDomain { get; set; }
    [J("is_meta")] public bool IsMeta { get; set; }
    [J("category")] public object Category { get; set; }
    [J("secure_media_embed")] public RedditMediaEmbed SecureRedditMediaEmbed { get; set; }
    [J("link_flair_text")] public object LinkFlairText { get; set; }
    [J("can_mod_post")] public bool CanModPost { get; set; }
    [J("score")] public long Score { get; set; }
    [J("approved_by")] public object ApprovedBy { get; set; }
    [J("is_created_from_ads_ui")] public bool IsCreatedFromAdsUi { get; set; }
    [J("author_premium")] public bool AuthorPremium { get; set; }
    [J("thumbnail")] public Uri Thumbnail { get; set; }
    [J("edited")] public bool Edited { get; set; }
    [J("author_flair_css_class")] public object AuthorFlairCssClass { get; set; }
    [J("author_flair_richtext")] public RedditAuthorFlairRichtext[] AuthorFlairRichtext { get; set; }
    [J("gildings")] public RedditGildings RedditGildings { get; set; }
    [J("post_hint")] public string PostHint { get; set; }
    [J("content_categories")] public object ContentCategories { get; set; }
    [J("is_self")] public bool IsSelf { get; set; }
    [J("mod_note")] public object ModNote { get; set; }
    [J("created")] public long Created { get; set; }
    [J("link_flair_type")] public string LinkFlairType { get; set; }
    [J("wls")] public long? Wls { get; set; }
    [J("removed_by_category")] public object RemovedByCategory { get; set; }
    [J("banned_by")] public object BannedBy { get; set; }
    [J("author_flair_type")] public string AuthorFlairType { get; set; }
    [J("domain")] public string Domain { get; set; }
    [J("allow_live_comments")] public bool AllowLiveComments { get; set; }
    [J("selftext_html")] public object SelftextHtml { get; set; }
    [J("likes")] public object Likes { get; set; }
    [J("suggested_sort")] public object SuggestedSort { get; set; }
    [J("banned_at_utc")] public object BannedAtUtc { get; set; }
    [J("url_overridden_by_dest")] public Uri UrlOverriddenByDest { get; set; }
    [J("view_count")] public object ViewCount { get; set; }
    [J("archived")] public bool Archived { get; set; }
    [J("no_follow")] public bool NoFollow { get; set; }
    [J("is_crosspostable")] public bool IsCrosspostable { get; set; }
    [J("pinned")] public bool Pinned { get; set; }
    [J("over_18")] public bool Over18 { get; set; }
    [J("preview")] public RedditPreview RedditPreview { get; set; }
    // [J("all_awardings")] public RedditAllAwarding[] AllAwardings { get; set; }
    [J("awarders")] public object[] Awarders { get; set; }
    [J("media_only")] public bool MediaOnly { get; set; }
    [J("can_gild")] public bool CanGild { get; set; }
    [J("spoiler")] public bool Spoiler { get; set; }
    [J("locked")] public bool Locked { get; set; }
    [J("author_flair_text")] public string AuthorFlairText { get; set; }
    [J("treatment_tags")] public object[] TreatmentTags { get; set; }
    [J("visited")] public bool Visited { get; set; }
    [J("removed_by")] public object RemovedBy { get; set; }
    [J("num_reports")] public object NumReports { get; set; }
    [J("distinguished")] public object Distinguished { get; set; }
    [J("subreddit_id")] public string SubredditId { get; set; }
    [J("author_is_blocked")] public bool AuthorIsBlocked { get; set; }
    [J("mod_reason_by")] public object ModReasonBy { get; set; }
    [J("removal_reason")] public object? RemovalReason { get; set; }
    [J("link_flair_background_color")] public string LinkFlairBackgroundColor { get; set; }
    [J("id")] public string Id { get; set; }
    [J("is_robot_indexable")] public bool IsRobotIndexable { get; set; }
    [J("report_reasons")] public object ReportReasons { get; set; }
    [J("author")] public string Author { get; set; }
    [J("discussion_type")] public object DiscussionType { get; set; }
    [J("num_comments")] public long NumComments { get; set; }
    [J("send_replies")] public bool SendReplies { get; set; }
    [J("whitelist_status")] public string WhitelistStatus { get; set; }
    [J("contest_mode")] public bool ContestMode { get; set; }
    [J("mod_reports")] public object[] ModReports { get; set; }
    [J("author_patreon_flair")] public bool AuthorPatreonFlair { get; set; }
    [J("author_flair_text_color")] public string AuthorFlairTextColor { get; set; }
    [J("permalink")] public string Permalink { get; set; }
    [J("parent_whitelist_status")] public string ParentWhitelistStatus { get; set; }
    [J("stickied")] public bool Stickied { get; set; }
    [J("url")] public string? Url { get; set; }
    [J("subreddit_subscribers")] public long SubredditSubscribers { get; set; }
    [J("created_utc")] public long CreatedUtc { get; set; }
    [J("num_crossposts")] public long NumCrossposts { get; set; }
    [J("media")] public RedditMedia RedditMedia { get; set; }
    [J("is_video")] public bool IsVideo { get; set; }
}

// public class RedditAllAwarding
// {
//     [J("giver_coin_reward")] public long? GiverCoinReward { get; set; }
//     [J("subreddit_id")] public object SubredditId { get; set; }
//     [J("is_new")] public bool IsNew { get; set; }
//     [J("days_of_drip_extension")] public long? DaysOfDripExtension { get; set; }
//     [J("coin_price")] public long CoinPrice { get; set; }
//     [J("id")] public string Id { get; set; }
//     [J("penny_donate")] public long? PennyDonate { get; set; }
//     [J("award_sub_type")] public string AwardSubType { get; set; }
//     [J("coin_reward")] public long CoinReward { get; set; }
//     [J("icon_url")] public Uri IconUrl { get; set; }
//     [J("days_of_premium")] public long DaysOfPremium { get; set; }
//     [J("tiers_by_required_awardings")] public object TiersByRequiredAwardings { get; set; }
//     [J("resized_icons")] public RedditResizedIcon[] ResizedIcons { get; set; }
//     [J("icon_width")] public long IconWidth { get; set; }
//     [J("static_icon_width")] public long StaticIconWidth { get; set; }
//     [J("start_date")] public object StartDate { get; set; }
//     [J("is_enabled")] public bool IsEnabled { get; set; }
//
//     [J("awardings_required_to_grant_benefits")]
//     public object AwardingsRequiredToGrantBenefits { get; set; }
//
//     [J("description")] public string Description { get; set; }
//     [J("end_date")] public object EndDate { get; set; }
//     [J("subreddit_coin_reward")] public long SubredditCoinReward { get; set; }
//     [J("count")] public long Count { get; set; }
//     [J("static_icon_height")] public long StaticIconHeight { get; set; }
//     [J("name")] public string Name { get; set; }
//     [J("resized_static_icons")] public RedditResizedIcon[] ResizedStaticIcons { get; set; }
//     [J("icon_format")] public string IconFormat { get; set; }
//     [J("icon_height")] public long IconHeight { get; set; }
//     [J("penny_price")] public long? PennyPrice { get; set; }
//     [J("award_type")] public string AwardType { get; set; }
//     [J("static_icon_url")] public Uri StaticIconUrl { get; set; }
// }

public class RedditResizedIcon
{
    [J("url")] public Uri Url { get; set; }
    [J("width")] public long Width { get; set; }
    [J("height")] public long Height { get; set; }
}

public class RedditAuthorFlairRichtext
{
    [J("e")] public string E { get; set; }
    [J("t")] public string T { get; set; }
}

public class RedditGildings
{
    [J("gid_1")] public long Gid1 { get; set; }
}

public class RedditMedia
{
    [J("reddit_video")] public RedditVideo RedditVideo { get; set; }
}

public class RedditVideo
{
    [J("bitrate_kbps")] public long BitrateKbps { get; set; }
    [J("fallback_url")] public Uri FallbackUrl { get; set; }
    [J("height")] public long Height { get; set; }
    [J("width")] public long Width { get; set; }
    [J("scrubber_media_url")] public Uri ScrubberMediaUrl { get; set; }
    [J("dash_url")] public Uri DashUrl { get; set; }
    [J("duration")] public long Duration { get; set; }
    [J("hls_url")] public Uri HlsUrl { get; set; }
    [J("is_gif")] public bool IsGif { get; set; }
    [J("transcoding_status")] public string TranscodingStatus { get; set; }
}

public class RedditMediaEmbed
{
}

public class RedditPreview
{
    [J("images")] public RedditImage[] Images { get; set; }
    [J("enabled")] public bool Enabled { get; set; }
}

public class RedditImage
{
    [J("source")] public RedditImageSource Source { get; set; }
    [J("resolutions")] public RedditImageSource[] Resolutions { get; set; }
    [J("variants")] public RedditVariants RedditVariants { get; set; }
    [J("id")] public string Id { get; set; }
}

public class RedditImageSource
{
    [J("url")] public Uri Url { get; set; }
    [J("width")] public long Width { get; set; }
    [J("height")] public long Height { get; set; }
}

public class RedditVariants
{
    [J("gif", NullValueHandling = N.Ignore)]
    public RedditGif RedditGif { get; set; }

    [J("mp4", NullValueHandling = N.Ignore)]
    public RedditGif Mp4 { get; set; }
}

public class RedditGif
{
    [J("source")] public RedditGifSource Source { get; set; }
    [J("resolutions")] public RedditGifSource[] Resolutions { get; set; }
}

public class RedditGifSource
{
    [J("url")] public Uri Url { get; set; }
    [J("width")] public long Width { get; set; }
    [J("height")] public long Height { get; set; }
}