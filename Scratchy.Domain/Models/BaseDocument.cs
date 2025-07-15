using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.Models;

/// <summary>
/// Base document class providing common fields and patterns for all MongoDB documents
/// </summary>
public abstract class BaseDocument
{
    /// <summary>
    /// MongoDB ObjectId as the primary key
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Document creation timestamp
    /// </summary>
    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Document last update timestamp
    /// </summary>
    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updates the UpdatedAt timestamp - call before saving changes
    /// </summary>
    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Base document class with soft delete capability
/// </summary>
public abstract class SoftDeleteDocument : BaseDocument
{
    /// <summary>
    /// Indicates if the document is soft deleted
    /// </summary>
    [BsonElement("isDeleted")]
    [BsonDefaultValue(false)]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Timestamp when the document was soft deleted
    /// </summary>
    [BsonElement("deletedAt")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Marks the document as deleted
    /// </summary>
    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    /// <summary>
    /// Restores a soft deleted document
    /// </summary>
    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        MarkAsUpdated();
    }
}
