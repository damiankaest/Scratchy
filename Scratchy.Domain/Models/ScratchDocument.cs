using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Scratch statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class ScratchStats
{
    [BsonElement("likesCount")]
    [BsonDefaultValue(0)]
    public int LikesCount { get; set; } = 0;

    [BsonElement("viewsCount")]
    [BsonDefaultValue(0)]
    public int ViewsCount { get; set; } = 0;

    [BsonElement("sharesCount")]
    [BsonDefaultValue(0)]
    public int SharesCount { get; set; } = 0;

    [BsonElement("commentsCount")]
    [BsonDefaultValue(0)]
    public int CommentsCount { get; set; } = 0;
}

/// <summary>
/// Main Scratch document model for MongoDB
/// Collection: scratches
/// </summary>
[BsonIgnoreExtraElements]
public class ScratchDocument : SoftDeleteDocument
{
    /// <summary>
    /// Scratch title
    /// </summary>
    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Scratch content/description
    /// </summary>
    [BsonElement("content")]
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Scratch rating (1-10 scale)
    /// </summary>
    [BsonElement("rating")]
    [Range(1, 10)]
    public int Rating { get; set; } = 5;

    /// <summary>
    /// Legacy like counter (maintained for compatibility)
    /// </summary>
    [BsonElement("likeCounter")]
    [BsonDefaultValue(0)]
    public int LikeCounter { get; set; } = 0;

    /// <summary>
    /// Scratch image URL (uploaded image)
    /// </summary>
    [BsonElement("scratchImageUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? ScratchImageUrl { get; set; }

    /// <summary>
    /// Scratch author (denormalized user reference)
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
    /// Scratch statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public ScratchStats Stats { get; set; } = new();

    /// <summary>
    /// Scratch tags for categorization
    /// </summary>
    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Scratch visibility (public, private, followers)
    /// </summary>
    [BsonElement("visibility")]
    [StringLength(20)]
    [BsonDefaultValue("public")]
    public string Visibility { get; set; } = "public";

    /// <summary>
    /// Scratch category/type
    /// </summary>
    [BsonElement("category")]
    [StringLength(50)]
    [BsonDefaultValue("general")]
    public string Category { get; set; } = "general";

    /// <summary>
    /// Featured flag for highlighted scratches
    /// </summary>
    [BsonElement("isFeatured")]
    [BsonDefaultValue(false)]
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// Moderator approval status
    /// </summary>
    [BsonElement("isApproved")]
    [BsonDefaultValue(true)]
    public bool IsApproved { get; set; } = true;

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
    /// Sets the scratch author
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
    /// Adds a tag to the scratch
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
    /// Removes a tag from the scratch
    /// </summary>
    /// <param name="tag">Tag to remove</param>
    public void RemoveTag(string tag)
    {
        var existingTag = Tags.FirstOrDefault(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase));
        if (existingTag != null)
        {
            Tags.Remove(existingTag);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates scratch statistics
    /// </summary>
    /// <param name="likesCount">Number of likes</param>
    /// <param name="viewsCount">Number of views</param>
    /// <param name="sharesCount">Number of shares</param>
    /// <param name="commentsCount">Number of comments</param>
    public void UpdateStats(int? likesCount = null, int? viewsCount = null, 
        int? sharesCount = null, int? commentsCount = null)
    {
        if (likesCount.HasValue) 
        {
            Stats.LikesCount = likesCount.Value;
            LikeCounter = likesCount.Value; // Keep legacy field in sync
        }
        if (viewsCount.HasValue) Stats.ViewsCount = viewsCount.Value;
        if (sharesCount.HasValue) Stats.SharesCount = sharesCount.Value;
        if (commentsCount.HasValue) Stats.CommentsCount = commentsCount.Value;

        MarkAsUpdated();
    }

    /// <summary>
    /// Increments the view count
    /// </summary>
    public void IncrementViews()
    {
        Stats.ViewsCount++;
        MarkAsUpdated();
    }

    /// <summary>
    /// Toggles featured status
    /// </summary>
    /// <param name="featured">Featured status</param>
    public void SetFeatured(bool featured)
    {
        IsFeatured = featured;
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets approval status
    /// </summary>
    /// <param name="approved">Approval status</param>
    public void SetApproval(bool approved)
    {
        IsApproved = approved;
        MarkAsUpdated();
    }
}
