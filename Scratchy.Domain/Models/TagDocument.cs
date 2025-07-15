using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Tag document model for MongoDB
/// Collection: tags
/// </summary>
[BsonIgnoreExtraElements]
public class TagDocument : BaseDocument
{
    /// <summary>
    /// Tag name (unique)
    /// </summary>
    [BsonElement("name")]
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized tag name for searching (lowercase, no spaces)
    /// </summary>
    [BsonElement("normalizedName")]
    [Required]
    [StringLength(50)]
    public string NormalizedName { get; set; } = string.Empty;

    /// <summary>
    /// Tag description
    /// </summary>
    [BsonElement("description")]
    [StringLength(200)]
    [BsonIgnoreIfNull]
    public string? Description { get; set; }

    /// <summary>
    /// Tag color (hex color code)
    /// </summary>
    [BsonElement("color")]
    [StringLength(7)]
    [BsonDefaultValue("#0066cc")]
    public string Color { get; set; } = "#0066cc";

    /// <summary>
    /// Number of times this tag has been used
    /// </summary>
    [BsonElement("usageCount")]
    [BsonDefaultValue(0)]
    public int UsageCount { get; set; } = 0;

    /// <summary>
    /// Tag category (e.g., "genre", "mood", "instrument", "style")
    /// </summary>
    [BsonElement("category")]
    [StringLength(50)]
    [BsonIgnoreIfNull]
    public string? Category { get; set; }

    /// <summary>
    /// Flag indicating if the tag is trending
    /// </summary>
    [BsonElement("isTrending")]
    [BsonDefaultValue(false)]
    public bool IsTrending { get; set; } = false;

    /// <summary>
    /// Flag indicating if the tag is featured/promoted
    /// </summary>
    [BsonElement("isFeatured")]
    [BsonDefaultValue(false)]
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// Related tags (for suggestions)
    /// </summary>
    [BsonElement("relatedTags")]
    public List<string> RelatedTags { get; set; } = new();

    /// <summary>
    /// Sets the normalized name based on the tag name
    /// </summary>
    public void SetNormalizedName()
    {
        NormalizedName = Name.ToLowerInvariant().Replace(" ", "").Replace("-", "").Replace("_", "");
        MarkAsUpdated();
    }

    /// <summary>
    /// Increments the usage count
    /// </summary>
    public void IncrementUsage()
    {
        UsageCount++;
        MarkAsUpdated();
    }

    /// <summary>
    /// Decrements the usage count
    /// </summary>
    public void DecrementUsage()
    {
        if (UsageCount > 0)
        {
            UsageCount--;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Adds a related tag
    /// </summary>
    /// <param name="tagName">Related tag name</param>
    public void AddRelatedTag(string tagName)
    {
        if (!RelatedTags.Contains(tagName, StringComparer.OrdinalIgnoreCase))
        {
            RelatedTags.Add(tagName);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a related tag
    /// </summary>
    /// <param name="tagName">Related tag name to remove</param>
    public void RemoveRelatedTag(string tagName)
    {
        var removed = RelatedTags.RemoveAll(t => t.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        if (removed > 0)
        {
            MarkAsUpdated();
        }
    }
}
