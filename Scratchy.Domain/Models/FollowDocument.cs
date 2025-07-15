using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.Models;

/// <summary>
/// Follow relationship document model for MongoDB
/// Collection: follows
/// Represents user follow relationships (User A follows User B)
/// </summary>
[BsonIgnoreExtraElements]
public class FollowDocument : BaseDocument
{
    /// <summary>
    /// User who is following (follower)
    /// </summary>
    [BsonElement("follower")]
    public UserReference Follower { get; set; } = new();

    /// <summary>
    /// User being followed (followed)
    /// </summary>
    [BsonElement("followed")]
    public UserReference Followed { get; set; } = new();

    /// <summary>
    /// Follow status (active, muted, blocked)
    /// </summary>
    [BsonElement("status")]
    [BsonDefaultValue("active")]
    public string Status { get; set; } = "active"; // "active", "muted", "blocked"

    /// <summary>
    /// Follow type (normal, close_friend, etc.)
    /// </summary>
    [BsonElement("followType")]
    [BsonDefaultValue("normal")]
    public string FollowType { get; set; } = "normal"; // "normal", "close_friend", "vip"

    /// <summary>
    /// Notification preferences for this follow relationship
    /// </summary>
    [BsonElement("notificationSettings")]
    public FollowNotificationSettings NotificationSettings { get; set; } = new();

    /// <summary>
    /// Metadata for additional follow relationship data
    /// </summary>
    [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Sets the follower user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetFollower(string userId, string username, string? profilePictureUrl = null)
    {
        Follower = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets the followed user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetFollowed(string userId, string username, string? profilePictureUrl = null)
    {
        Followed = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Updates the follow status
    /// </summary>
    /// <param name="status">New status</param>
    public void UpdateStatus(string status)
    {
        Status = status;
        MarkAsUpdated();
    }

    /// <summary>
    /// Updates the follow type
    /// </summary>
    /// <param name="followType">New follow type</param>
    public void UpdateFollowType(string followType)
    {
        FollowType = followType;
        MarkAsUpdated();
    }

    /// <summary>
    /// Mutes the follow relationship
    /// </summary>
    public void Mute()
    {
        Status = "muted";
        MarkAsUpdated();
    }

    /// <summary>
    /// Unmutes the follow relationship
    /// </summary>
    public void Unmute()
    {
        Status = "active";
        MarkAsUpdated();
    }

    /// <summary>
    /// Blocks the follow relationship
    /// </summary>
    public void Block()
    {
        Status = "blocked";
        MarkAsUpdated();
    }
}

/// <summary>
/// Follow notification settings embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class FollowNotificationSettings
{
    [BsonElement("notifyOnPosts")]
    [BsonDefaultValue(true)]
    public bool NotifyOnPosts { get; set; } = true;

    [BsonElement("notifyOnScratches")]
    [BsonDefaultValue(true)]
    public bool NotifyOnScratches { get; set; } = true;

    [BsonElement("notifyOnPlaylists")]
    [BsonDefaultValue(false)]
    public bool NotifyOnPlaylists { get; set; } = false;

    [BsonElement("notifyOnLive")]
    [BsonDefaultValue(false)]
    public bool NotifyOnLive { get; set; } = false;
}
