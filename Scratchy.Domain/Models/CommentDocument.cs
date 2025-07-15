using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Comment document model for MongoDB
/// Collection: comments
/// </summary>
[BsonIgnoreExtraElements]
public class CommentDocument : BaseDocument
{
    /// <summary>
    /// Comment content
    /// </summary>
    [BsonElement("content")]
    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// ID of the user who made the comment
    /// </summary>
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Username for quick access (denormalized for performance)
    /// </summary>
    [BsonElement("username")]
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User profile picture URL (denormalized for performance)
    /// </summary>
    [BsonElement("userProfilePictureUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? UserProfilePictureUrl { get; set; }

    /// <summary>
    /// ID of the post this comment belongs to
    /// </summary>
    [BsonElement("postId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string PostId { get; set; } = string.Empty;

    /// <summary>
    /// Parent comment ID for nested comments (replies)
    /// </summary>
    [BsonElement("parentCommentId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? ParentCommentId { get; set; }

    /// <summary>
    /// Number of likes on this comment
    /// </summary>
    [BsonElement("likeCount")]
    [BsonDefaultValue(0)]
    public int LikeCount { get; set; } = 0;

    /// <summary>
    /// Number of replies to this comment
    /// </summary>
    [BsonElement("replyCount")]
    [BsonDefaultValue(0)]
    public int ReplyCount { get; set; } = 0;

    /// <summary>
    /// Array of user IDs who liked this comment
    /// </summary>
    [BsonElement("likedByUsers")]
    public List<string> LikedByUsers { get; set; } = new();

    /// <summary>
    /// Flag indicating if the comment has been edited
    /// </summary>
    [BsonElement("isEdited")]
    [BsonDefaultValue(false)]
    public bool IsEdited { get; set; } = false;

    /// <summary>
    /// Timestamp when the comment was last edited
    /// </summary>
    [BsonElement("editedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonIgnoreIfNull]
    public DateTime? EditedAt { get; set; }

    /// <summary>
    /// Flag indicating if the comment is hidden/deleted
    /// </summary>
    [BsonElement("isDeleted")]
    [BsonDefaultValue(false)]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Adds a like from a user
    /// </summary>
    /// <param name="userId">User ID who liked the comment</param>
    public void AddLike(string userId)
    {
        if (!LikedByUsers.Contains(userId))
        {
            LikedByUsers.Add(userId);
            LikeCount = LikedByUsers.Count;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a like from a user
    /// </summary>
    /// <param name="userId">User ID who unliked the comment</param>
    public void RemoveLike(string userId)
    {
        if (LikedByUsers.Remove(userId))
        {
            LikeCount = LikedByUsers.Count;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Marks the comment as edited
    /// </summary>
    public void MarkAsEdited()
    {
        IsEdited = true;
        EditedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    /// <summary>
    /// Soft deletes the comment
    /// </summary>
    public void SoftDelete()
    {
        IsDeleted = true;
        Content = "[Comment deleted]";
        MarkAsUpdated();
    }

    /// <summary>
    /// Increments the reply count
    /// </summary>
    public void IncrementReplyCount()
    {
        ReplyCount++;
        MarkAsUpdated();
    }

    /// <summary>
    /// Decrements the reply count
    /// </summary>
    public void DecrementReplyCount()
    {
        if (ReplyCount > 0)
        {
            ReplyCount--;
            MarkAsUpdated();
        }
    }
}
