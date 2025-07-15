using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Notification metadata embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class NotificationMetadata
{
    [BsonElement("relatedEntityId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? RelatedEntityId { get; set; }

    [BsonElement("relatedEntityType")]
    [StringLength(50)]
    [BsonIgnoreIfNull]
    public string? RelatedEntityType { get; set; } // "post", "scratch", "playlist", "user", etc.

    [BsonElement("additionalData")]
    [BsonIgnoreIfNull]
    public Dictionary<string, object>? AdditionalData { get; set; }
}

/// <summary>
/// Main Notification document model for MongoDB
/// Collection: notifications
/// </summary>
[BsonIgnoreExtraElements]
public class NotificationDocument : BaseDocument
{
    /// <summary>
    /// Notification recipient (denormalized user reference)
    /// </summary>
    [BsonElement("recipient")]
    public UserReference Recipient { get; set; } = new();

    /// <summary>
    /// Notification sender (denormalized user reference, optional)
    /// </summary>
    [BsonElement("sender")]
    [BsonIgnoreIfNull]
    public UserReference? Sender { get; set; }

    /// <summary>
    /// Notification type/category
    /// </summary>
    [BsonElement("type")]
    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Notification message/content
    /// </summary>
    [BsonElement("message")]
    [Required]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Notification title (optional)
    /// </summary>
    [BsonElement("title")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? Title { get; set; }

    /// <summary>
    /// Read status
    /// </summary>
    [BsonElement("isRead")]
    [BsonDefaultValue(false)]
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Timestamp when notification was read
    /// </summary>
    [BsonElement("readAt")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Notification priority level
    /// </summary>
    [BsonElement("priority")]
    [StringLength(20)]
    [BsonDefaultValue("normal")]
    public string Priority { get; set; } = "normal"; // "low", "normal", "high", "urgent"

    /// <summary>
    /// Notification delivery status
    /// </summary>
    [BsonElement("deliveryStatus")]
    [StringLength(20)]
    [BsonDefaultValue("pending")]
    public string DeliveryStatus { get; set; } = "pending"; // "pending", "sent", "delivered", "failed"

    /// <summary>
    /// Notification delivery method
    /// </summary>
    [BsonElement("deliveryMethod")]
    [StringLength(20)]
    [BsonDefaultValue("in-app")]
    public string DeliveryMethod { get; set; } = "in-app"; // "in-app", "push", "email", "sms"

    /// <summary>
    /// Expiration date for temporary notifications
    /// </summary>
    [BsonElement("expiresAt")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Action URL for clickable notifications
    /// </summary>
    [BsonElement("actionUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? ActionUrl { get; set; }

    /// <summary>
    /// Notification metadata for additional context
    /// </summary>
    [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public NotificationMetadata? Metadata { get; set; }

    /// <summary>
    /// Notification category for grouping
    /// </summary>
    [BsonElement("category")]
    [StringLength(50)]
    [BsonDefaultValue("general")]
    public string Category { get; set; } = "general"; // "social", "system", "music", "general"

    /// <summary>
    /// Marks the notification as read
    /// </summary>
    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Marks the notification as unread
    /// </summary>
    public void MarkAsUnread()
    {
        if (IsRead)
        {
            IsRead = false;
            ReadAt = null;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Sets the notification recipient
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetRecipient(string userId, string username, string? profilePictureUrl = null)
    {
        Recipient = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets the notification sender
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetSender(string userId, string username, string? profilePictureUrl = null)
    {
        Sender = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets notification metadata
    /// </summary>
    /// <param name="relatedEntityId">Related entity ID</param>
    /// <param name="relatedEntityType">Related entity type</param>
    /// <param name="additionalData">Additional metadata</param>
    public void SetMetadata(string? relatedEntityId = null, string? relatedEntityType = null, 
        Dictionary<string, object>? additionalData = null)
    {
        Metadata = new NotificationMetadata
        {
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType,
            AdditionalData = additionalData
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Updates delivery status
    /// </summary>
    /// <param name="status">New delivery status</param>
    public void UpdateDeliveryStatus(string status)
    {
        DeliveryStatus = status;
        MarkAsUpdated();
    }

    /// <summary>
    /// Checks if the notification has expired
    /// </summary>
    /// <returns>True if expired, false otherwise</returns>
    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
    }

    /// <summary>
    /// Sets expiration date
    /// </summary>
    /// <param name="expirationDate">Expiration date</param>
    public void SetExpiration(DateTime expirationDate)
    {
        ExpiresAt = expirationDate;
        MarkAsUpdated();
    }

    /// <summary>
    /// Creates a follow notification
    /// </summary>
    /// <param name="recipientId">Recipient user ID</param>
    /// <param name="recipientUsername">Recipient username</param>
    /// <param name="senderId">Sender user ID</param>
    /// <param name="senderUsername">Sender username</param>
    /// <param name="senderProfilePicture">Sender profile picture</param>
    /// <returns>Follow notification</returns>
    public static NotificationDocument CreateFollowNotification(string recipientId, string recipientUsername,
        string senderId, string senderUsername, string? senderProfilePicture = null)
    {
        var notification = new NotificationDocument
        {
            Type = "follow",
            Category = "social",
            Title = "New Follower",
            Message = $"{senderUsername} started following you",
            Priority = "normal"
        };

        notification.SetRecipient(recipientId, recipientUsername);
        notification.SetSender(senderId, senderUsername, senderProfilePicture);
        notification.SetMetadata(senderId, "user");

        return notification;
    }

    /// <summary>
    /// Creates a like notification
    /// </summary>
    /// <param name="recipientId">Recipient user ID</param>
    /// <param name="recipientUsername">Recipient username</param>
    /// <param name="senderId">Sender user ID</param>
    /// <param name="senderUsername">Sender username</param>
    /// <param name="entityId">Liked entity ID</param>
    /// <param name="entityType">Liked entity type (post, scratch)</param>
    /// <param name="senderProfilePicture">Sender profile picture</param>
    /// <returns>Like notification</returns>
    public static NotificationDocument CreateLikeNotification(string recipientId, string recipientUsername,
        string senderId, string senderUsername, string entityId, string entityType, string? senderProfilePicture = null)
    {
        var notification = new NotificationDocument
        {
            Type = "like",
            Category = "social",
            Title = "New Like",
            Message = $"{senderUsername} liked your {entityType}",
            Priority = "normal"
        };

        notification.SetRecipient(recipientId, recipientUsername);
        notification.SetSender(senderId, senderUsername, senderProfilePicture);
        notification.SetMetadata(entityId, entityType);

        return notification;
    }
}
