using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// User settings embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class UserSettings
{
    [BsonElement("isPublicProfile")]
    [BsonDefaultValue(true)]
    public bool IsPublicProfile { get; set; } = true;

    [BsonElement("receiveNotifications")]
    [BsonDefaultValue(true)]
    public bool ReceiveNotifications { get; set; } = true;

    [BsonElement("language")]
    [BsonDefaultValue("en")]
    public string Language { get; set; } = "en";

    [BsonElement("theme")]
    [BsonDefaultValue("light")]
    public string Theme { get; set; } = "light";
}

/// <summary>
/// User statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class UserStats
{
    [BsonElement("followersCount")]
    [BsonDefaultValue(0)]
    public int FollowersCount { get; set; } = 0;

    [BsonElement("followingCount")]
    [BsonDefaultValue(0)]
    public int FollowingCount { get; set; } = 0;

    [BsonElement("postsCount")]
    [BsonDefaultValue(0)]
    public int PostsCount { get; set; } = 0;

    [BsonElement("scratchesCount")]
    [BsonDefaultValue(0)]
    public int ScratchesCount { get; set; } = 0;

    [BsonElement("playlistsCount")]
    [BsonDefaultValue(0)]
    public int PlaylistsCount { get; set; } = 0;
}

/// <summary>
/// User badge embedded document (replaces UserBadge junction table)
/// </summary>
[BsonIgnoreExtraElements]
public class UserBadgeEmbedded
{
    [BsonElement("badgeId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BadgeId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("iconUrl")]
    public string? IconUrl { get; set; }

    [BsonElement("awardedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime AwardedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Main User document model for MongoDB
/// Collection: users
/// </summary>
[BsonIgnoreExtraElements]
public class UserDocument : BaseDocument
{
    /// <summary>
    /// Firebase user ID for authentication
    /// </summary>
    [BsonElement("firebaseId")]
    [Required]
    [StringLength(50)]
    public string FirebaseId { get; set; } = string.Empty;

    /// <summary>
    /// Unique username
    /// </summary>
    [BsonElement("username")]
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User email address (optional, for non-Firebase authentication)
    /// </summary>
    [BsonElement("email")]
    [StringLength(100)]
    [EmailAddress]
    [BsonIgnoreIfNull]
    public string? Email { get; set; }

    /// <summary>
    /// Password hash (optional, for non-Firebase authentication)
    /// </summary>
    [BsonElement("passwordHash")]
    [StringLength(256)]
    [BsonIgnoreIfNull]
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Profile picture URL
    /// </summary>
    [BsonElement("profilePictureUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Embedded user settings (1:1 relationship)
    /// </summary>
    [BsonElement("settings")]
    public UserSettings Settings { get; set; } = new();

    /// <summary>
    /// Embedded user statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public UserStats Stats { get; set; } = new();

    /// <summary>
    /// User badges array (replaces M:M UserBadge table)
    /// </summary>
    [BsonElement("badges")]
    public List<UserBadgeEmbedded> Badges { get; set; } = new();

    /// <summary>
    /// Adds a badge to the user
    /// </summary>
    /// <param name="badgeId">Badge ID</param>
    /// <param name="name">Badge name</param>
    /// <param name="description">Badge description</param>
    /// <param name="iconUrl">Badge icon URL</param>
    public void AddBadge(string badgeId, string name, string description, string? iconUrl = null)
    {
        if (Badges.Any(b => b.BadgeId == badgeId))
            return; // Already has this badge

        Badges.Add(new UserBadgeEmbedded
        {
            BadgeId = badgeId,
            Name = name,
            Description = description,
            IconUrl = iconUrl,
            AwardedAt = DateTime.UtcNow
        });

        MarkAsUpdated();
    }

    /// <summary>
    /// Removes a badge from the user
    /// </summary>
    /// <param name="badgeId">Badge ID to remove</param>
    public void RemoveBadge(string badgeId)
    {
        var badge = Badges.FirstOrDefault(b => b.BadgeId == badgeId);
        if (badge != null)
        {
            Badges.Remove(badge);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates user statistics
    /// </summary>
    /// <param name="followersCount">Number of followers</param>
    /// <param name="followingCount">Number of following</param>
    /// <param name="postsCount">Number of posts</param>
    /// <param name="scratchesCount">Number of scratches</param>
    /// <param name="playlistsCount">Number of playlists</param>
    public void UpdateStats(int? followersCount = null, int? followingCount = null, 
        int? postsCount = null, int? scratchesCount = null, int? playlistsCount = null)
    {
        if (followersCount.HasValue) Stats.FollowersCount = followersCount.Value;
        if (followingCount.HasValue) Stats.FollowingCount = followingCount.Value;
        if (postsCount.HasValue) Stats.PostsCount = postsCount.Value;
        if (scratchesCount.HasValue) Stats.ScratchesCount = scratchesCount.Value;
        if (playlistsCount.HasValue) Stats.PlaylistsCount = playlistsCount.Value;

        MarkAsUpdated();
    }
}
