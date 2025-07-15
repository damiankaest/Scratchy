using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Badge criteria embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class BadgeCriteria
{
    [BsonElement("type")]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty; // "posts_count", "scratches_count", "followers_count", etc.

    [BsonElement("threshold")]
    public int Threshold { get; set; } = 0;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Badge statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class BadgeStats
{
    [BsonElement("totalAwarded")]
    [BsonDefaultValue(0)]
    public int TotalAwarded { get; set; } = 0;

    [BsonElement("lastAwarded")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? LastAwarded { get; set; }

    [BsonElement("popularityScore")]
    [BsonDefaultValue(0)]
    public int PopularityScore { get; set; } = 0;
}

/// <summary>
/// Main Badge document model for MongoDB
/// Collection: badges
/// System-wide badges that can be awarded to users
/// </summary>
[BsonIgnoreExtraElements]
public class BadgeDocument : BaseDocument
{
    /// <summary>
    /// Badge name/title
    /// </summary>
    [BsonElement("name")]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Badge description
    /// </summary>
    [BsonElement("description")]
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Badge icon/image URL
    /// </summary>
    [BsonElement("iconUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? IconUrl { get; set; }

    /// <summary>
    /// Badge category
    /// </summary>
    [BsonElement("category")]
    [StringLength(50)]
    [BsonDefaultValue("general")]
    public string Category { get; set; } = "general"; // "social", "music", "achievement", "general"

    /// <summary>
    /// Badge rarity level
    /// </summary>
    [BsonElement("rarity")]
    [StringLength(20)]
    [BsonDefaultValue("common")]
    public string Rarity { get; set; } = "common"; // "common", "rare", "epic", "legendary"

    /// <summary>
    /// Badge points value
    /// </summary>
    [BsonElement("points")]
    [BsonDefaultValue(10)]
    public int Points { get; set; } = 10;

    /// <summary>
    /// Badge color theme (hex color)
    /// </summary>
    [BsonElement("color")]
    [StringLength(7)]
    [BsonDefaultValue("#3498db")]
    public string Color { get; set; } = "#3498db";

    /// <summary>
    /// Whether badge is currently active/available
    /// </summary>
    [BsonElement("isActive")]
    [BsonDefaultValue(true)]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Badge can be automatically awarded
    /// </summary>
    [BsonElement("isAutoAwardable")]
    [BsonDefaultValue(false)]
    public bool IsAutoAwardable { get; set; } = false;

    /// <summary>
    /// Badge is hidden from public view
    /// </summary>
    [BsonElement("isHidden")]
    [BsonDefaultValue(false)]
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// Badge is limited edition (time-limited)
    /// </summary>
    [BsonElement("isLimitedEdition")]
    [BsonDefaultValue(false)]
    public bool IsLimitedEdition { get; set; } = false;

    /// <summary>
    /// Available until date (for limited edition badges)
    /// </summary>
    [BsonElement("availableUntil")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? AvailableUntil { get; set; }

    /// <summary>
    /// Badge award criteria (for auto-awarding)
    /// </summary>
    [BsonElement("criteria")]
    public List<BadgeCriteria> Criteria { get; set; } = new();

    /// <summary>
    /// Badge statistics
    /// </summary>
    [BsonElement("stats")]
    public BadgeStats Stats { get; set; } = new();

    /// <summary>
    /// Badge tags for searching/filtering
    /// </summary>
    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Badge unlock requirements description
    /// </summary>
    [BsonElement("unlockDescription")]
    [BsonIgnoreIfNull]
    public string? UnlockDescription { get; set; }

    /// <summary>
    /// Sets badge as active/inactive
    /// </summary>
    /// <param name="active">Active status</param>
    public void SetActive(bool active)
    {
        IsActive = active;
        MarkAsUpdated();
    }

    /// <summary>
    /// Adds criteria for auto-awarding
    /// </summary>
    /// <param name="type">Criteria type</param>
    /// <param name="threshold">Threshold value</param>
    /// <param name="description">Criteria description</param>
    public void AddCriteria(string type, int threshold, string description)
    {
        if (!Criteria.Any(c => c.Type == type))
        {
            Criteria.Add(new BadgeCriteria
            {
                Type = type,
                Threshold = threshold,
                Description = description
            });
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes criteria
    /// </summary>
    /// <param name="type">Criteria type to remove</param>
    public void RemoveCriteria(string type)
    {
        var criteria = Criteria.FirstOrDefault(c => c.Type == type);
        if (criteria != null)
        {
            Criteria.Remove(criteria);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Adds a tag to the badge
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
    /// Updates badge statistics
    /// </summary>
    /// <param name="totalAwarded">Total awarded count</param>
    /// <param name="popularityScore">Popularity score</param>
    public void UpdateStats(int? totalAwarded = null, int? popularityScore = null)
    {
        if (totalAwarded.HasValue) Stats.TotalAwarded = totalAwarded.Value;
        if (popularityScore.HasValue) Stats.PopularityScore = popularityScore.Value;
        
        Stats.LastAwarded = DateTime.UtcNow;
        MarkAsUpdated();
    }

    /// <summary>
    /// Increments the awarded count
    /// </summary>
    public void IncrementAwarded()
    {
        Stats.TotalAwarded++;
        Stats.LastAwarded = DateTime.UtcNow;
        MarkAsUpdated();
    }

    /// <summary>
    /// Checks if badge is available (not expired)
    /// </summary>
    /// <returns>True if available, false if expired</returns>
    public bool IsAvailable()
    {
        return IsActive && (!IsLimitedEdition || !AvailableUntil.HasValue || AvailableUntil.Value > DateTime.UtcNow);
    }

    /// <summary>
    /// Sets limited edition availability
    /// </summary>
    /// <param name="availableUntil">Available until date</param>
    public void SetLimitedEdition(DateTime availableUntil)
    {
        IsLimitedEdition = true;
        AvailableUntil = availableUntil;
        MarkAsUpdated();
    }

    /// <summary>
    /// Creates a first post badge
    /// </summary>
    /// <returns>First post badge</returns>
    public static BadgeDocument CreateFirstPostBadge()
    {
        var badge = new BadgeDocument
        {
            Name = "First Post",
            Description = "Created your first post in the community",
            Category = "achievement",
            Rarity = "common",
            Points = 10,
            Color = "#2ecc71",
            IsAutoAwardable = true,
            UnlockDescription = "Share your first post"
        };

        badge.AddCriteria("posts_count", 1, "Create your first post");
        badge.AddTag("milestone");
        badge.AddTag("social");

        return badge;
    }

    /// <summary>
    /// Creates a music enthusiast badge
    /// </summary>
    /// <returns>Music enthusiast badge</returns>
    public static BadgeDocument CreateMusicEnthusiastBadge()
    {
        var badge = new BadgeDocument
        {
            Name = "Music Enthusiast",
            Description = "Shared 50 music scratches",
            Category = "music",
            Rarity = "rare",
            Points = 100,
            Color = "#9b59b6",
            IsAutoAwardable = true,
            UnlockDescription = "Share 50 music scratches"
        };

        badge.AddCriteria("scratches_count", 50, "Create 50 scratches");
        badge.AddTag("music");
        badge.AddTag("content");

        return badge;
    }
}
