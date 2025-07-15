using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// User reference embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class UserReference
{
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("username")]
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [BsonElement("profilePictureUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? ProfilePictureUrl { get; set; }
}

/// <summary>
/// Album reference embedded document for posts
/// </summary>
[BsonIgnoreExtraElements]
public class AlbumReference
{
    [BsonElement("albumId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AlbumId { get; set; } = string.Empty;

    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [BsonElement("coverImageUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? CoverImageUrl { get; set; }

    [BsonElement("artist")]
    public ArtistReference Artist { get; set; } = new();
}

/// <summary>
/// Comment embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class CommentEmbedded
{
    [BsonElement("commentId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CommentId { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("content")]
    [Required]
    public string Content { get; set; } = string.Empty;

    [BsonElement("user")]
    public UserReference User { get; set; } = new();

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("isDeleted")]
    [BsonDefaultValue(false)]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Marks the comment as updated
    /// </summary>
    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Post statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class PostStats
{
    [BsonElement("commentsCount")]
    [BsonDefaultValue(0)]
    public int CommentsCount { get; set; } = 0;

    [BsonElement("likesCount")]
    [BsonDefaultValue(0)]
    public int LikesCount { get; set; } = 0;

    [BsonElement("sharesCount")]
    [BsonDefaultValue(0)]
    public int SharesCount { get; set; } = 0;
}

/// <summary>
/// Main Post document model for MongoDB
/// Collection: posts
/// </summary>
[BsonIgnoreExtraElements]
public class PostDocument : SoftDeleteDocument
{
    /// <summary>
    /// Post content/text
    /// </summary>
    [BsonElement("content")]
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Album rating (1-5 stars, optional)
    /// </summary>
    [BsonElement("rating")]
    [BsonIgnoreIfNull]
    [Range(1, 5)]
    public decimal? Rating { get; set; }

    /// <summary>
    /// Post author (denormalized user reference)
    /// </summary>
    [BsonElement("user")]
    public UserReference User { get; set; } = new();

    /// <summary>
    /// Referenced album (optional)
    /// </summary>
    [BsonElement("album")]
    [BsonIgnoreIfNull]
    public AlbumReference? Album { get; set; }

    /// <summary>
    /// Post statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public PostStats Stats { get; set; } = new();

    /// <summary>
    /// Embedded comments array (1:M relationship embedded)
    /// </summary>
    [BsonElement("comments")]
    public List<CommentEmbedded> Comments { get; set; } = new();

    /// <summary>
    /// Post tags for categorization
    /// </summary>
    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Post images/media URLs
    /// </summary>
    [BsonElement("mediaUrls")]
    public List<string> MediaUrls { get; set; } = new();

    /// <summary>
    /// Post visibility (public, private, followers)
    /// </summary>
    [BsonElement("visibility")]
    [StringLength(20)]
    [BsonDefaultValue("public")]
    public string Visibility { get; set; } = "public";

    /// <summary>
    /// Adds a comment to the post
    /// </summary>
    /// <param name="content">Comment content</param>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">User profile picture URL</param>
    /// <returns>The created comment</returns>
    public CommentEmbedded AddComment(string content, string userId, string username, string? profilePictureUrl = null)
    {
        var comment = new CommentEmbedded
        {
            Content = content,
            User = new UserReference
            {
                UserId = userId,
                Username = username,
                ProfilePictureUrl = profilePictureUrl
            }
        };

        Comments.Add(comment);
        Stats.CommentsCount = Comments.Count(c => !c.IsDeleted);
        MarkAsUpdated();

        return comment;
    }

    /// <summary>
    /// Removes a comment from the post (soft delete)
    /// </summary>
    /// <param name="commentId">Comment ID to remove</param>
    public void RemoveComment(string commentId)
    {
        var comment = Comments.FirstOrDefault(c => c.CommentId == commentId);
        if (comment != null)
        {
            comment.IsDeleted = true;
            comment.MarkAsUpdated();
            Stats.CommentsCount = Comments.Count(c => !c.IsDeleted);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates a comment
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="newContent">New comment content</param>
    public void UpdateComment(string commentId, string newContent)
    {
        var comment = Comments.FirstOrDefault(c => c.CommentId == commentId && !c.IsDeleted);
        if (comment != null)
        {
            comment.Content = newContent;
            comment.MarkAsUpdated();
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Sets the referenced album
    /// </summary>
    /// <param name="albumId">Album ID</param>
    /// <param name="albumTitle">Album title</param>
    /// <param name="coverImageUrl">Album cover URL</param>
    /// <param name="artistId">Artist ID</param>
    /// <param name="artistName">Artist name</param>
    /// <param name="artistSpotifyId">Artist Spotify ID</param>
    public void SetAlbum(string albumId, string albumTitle, string? coverImageUrl = null, 
        string? artistId = null, string? artistName = null, string? artistSpotifyId = null)
    {
        Album = new AlbumReference
        {
            AlbumId = albumId,
            Title = albumTitle,
            CoverImageUrl = coverImageUrl,
            Artist = new ArtistReference
            {
                ArtistId = artistId ?? string.Empty,
                Name = artistName ?? string.Empty,
                SpotifyId = artistSpotifyId
            }
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets the post author
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetUser(string userId, string username, string? profilePictureUrl = null)
    {
        User = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a tag to the post
    /// </summary>
    /// <param name="tag">Tag to add</param>
    public void AddTag(string tag)
    {
        if (!string.IsNullOrWhiteSpace(tag) && !Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
        {
            Tags.Add(tag);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates post statistics
    /// </summary>
    /// <param name="likesCount">Number of likes</param>
    /// <param name="sharesCount">Number of shares</param>
    public void UpdateStats(int? likesCount = null, int? sharesCount = null)
    {
        if (likesCount.HasValue) Stats.LikesCount = likesCount.Value;
        if (sharesCount.HasValue) Stats.SharesCount = sharesCount.Value;
        
        // Always recalculate comments count from actual comments
        Stats.CommentsCount = Comments.Count(c => !c.IsDeleted);
        
        MarkAsUpdated();
    }
}
