using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Genre document model for MongoDB
/// Collection: genres
/// </summary>
[BsonIgnoreExtraElements]
public class GenreDocument : BaseDocument
{
    /// <summary>
    /// Genre name (unique)
    /// </summary>
    [BsonElement("name")]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized genre name for searching (lowercase, no spaces)
    /// </summary>
    [BsonElement("normalizedName")]
    [Required]
    [StringLength(100)]
    public string NormalizedName { get; set; } = string.Empty;

    /// <summary>
    /// Genre description
    /// </summary>
    [BsonElement("description")]
    [StringLength(500)]
    [BsonIgnoreIfNull]
    public string? Description { get; set; }

    /// <summary>
    /// Parent genre ID for genre hierarchy
    /// </summary>
    [BsonElement("parentGenreId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? ParentGenreId { get; set; }

    /// <summary>
    /// Sub-genres (child genres)
    /// </summary>
    [BsonElement("subGenres")]
    public List<string> SubGenres { get; set; } = new();

    /// <summary>
    /// Spotify genre ID for integration
    /// </summary>
    [BsonElement("spotifyGenreId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyGenreId { get; set; }

    /// <summary>
    /// Genre color for UI representation (hex color code)
    /// </summary>
    [BsonElement("color")]
    [StringLength(7)]
    [BsonDefaultValue("#6c757d")]
    public string Color { get; set; } = "#6c757d";

    /// <summary>
    /// Genre icon/emoji for UI representation
    /// </summary>
    [BsonElement("icon")]
    [StringLength(10)]
    [BsonIgnoreIfNull]
    public string? Icon { get; set; }

    /// <summary>
    /// Number of artists in this genre
    /// </summary>
    [BsonElement("artistCount")]
    [BsonDefaultValue(0)]
    public int ArtistCount { get; set; } = 0;

    /// <summary>
    /// Number of albums in this genre
    /// </summary>
    [BsonElement("albumCount")]
    [BsonDefaultValue(0)]
    public int AlbumCount { get; set; } = 0;

    /// <summary>
    /// Number of tracks in this genre
    /// </summary>
    [BsonElement("trackCount")]
    [BsonDefaultValue(0)]
    public int TrackCount { get; set; } = 0;

    /// <summary>
    /// Genre popularity score (0-100)
    /// </summary>
    [BsonElement("popularity")]
    [BsonDefaultValue(0)]
    public int Popularity { get; set; } = 0;

    /// <summary>
    /// Flag indicating if the genre is trending
    /// </summary>
    [BsonElement("isTrending")]
    [BsonDefaultValue(false)]
    public bool IsTrending { get; set; } = false;

    /// <summary>
    /// Flag indicating if the genre is featured
    /// </summary>
    [BsonElement("isFeatured")]
    [BsonDefaultValue(false)]
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// Keywords associated with this genre for search
    /// </summary>
    [BsonElement("keywords")]
    public List<string> Keywords { get; set; } = new();

    /// <summary>
    /// Sets the normalized name based on the genre name
    /// </summary>
    public void SetNormalizedName()
    {
        NormalizedName = Name.ToLowerInvariant().Replace(" ", "").Replace("-", "").Replace("_", "");
        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a sub-genre
    /// </summary>
    /// <param name="subGenreName">Sub-genre name</param>
    public void AddSubGenre(string subGenreName)
    {
        if (!SubGenres.Contains(subGenreName, StringComparer.OrdinalIgnoreCase))
        {
            SubGenres.Add(subGenreName);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a sub-genre
    /// </summary>
    /// <param name="subGenreName">Sub-genre name to remove</param>
    public void RemoveSubGenre(string subGenreName)
    {
        var removed = SubGenres.RemoveAll(g => g.Equals(subGenreName, StringComparison.OrdinalIgnoreCase));
        if (removed > 0)
        {
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates genre statistics
    /// </summary>
    /// <param name="artistCount">Number of artists</param>
    /// <param name="albumCount">Number of albums</param>
    /// <param name="trackCount">Number of tracks</param>
    public void UpdateStats(int? artistCount = null, int? albumCount = null, int? trackCount = null)
    {
        if (artistCount.HasValue) ArtistCount = artistCount.Value;
        if (albumCount.HasValue) AlbumCount = albumCount.Value;
        if (trackCount.HasValue) TrackCount = trackCount.Value;

        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a keyword for search optimization
    /// </summary>
    /// <param name="keyword">Keyword to add</param>
    public void AddKeyword(string keyword)
    {
        if (!Keywords.Contains(keyword, StringComparer.OrdinalIgnoreCase))
        {
            Keywords.Add(keyword.ToLowerInvariant());
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a keyword
    /// </summary>
    /// <param name="keyword">Keyword to remove</param>
    public void RemoveKeyword(string keyword)
    {
        var removed = Keywords.RemoveAll(k => k.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        if (removed > 0)
        {
            MarkAsUpdated();
        }
    }
}
