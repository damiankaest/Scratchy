using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Artist statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class ArtistStats
{
    [BsonElement("albumsCount")]
    [BsonDefaultValue(0)]
    public int AlbumsCount { get; set; } = 0;

    [BsonElement("tracksCount")]
    [BsonDefaultValue(0)]
    public int TracksCount { get; set; } = 0;

    [BsonElement("postsCount")]
    [BsonDefaultValue(0)]
    public int PostsCount { get; set; } = 0;

    [BsonElement("scratchesCount")]
    [BsonDefaultValue(0)]
    public int ScratchesCount { get; set; } = 0;
}

/// <summary>
/// Main Artist document model for MongoDB
/// Collection: artists
/// </summary>
[BsonIgnoreExtraElements]
public class ArtistDocument : BaseDocument
{
    /// <summary>
    /// Spotify artist ID (external reference)
    /// </summary>
    [BsonElement("spotifyId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }

    /// <summary>
    /// Artist name
    /// </summary>
    [BsonElement("name")]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Artist biography/description
    /// </summary>
    [BsonElement("bio")]
    [BsonIgnoreIfNull]
    public string? Bio { get; set; }

    /// <summary>
    /// Artist profile picture URL
    /// </summary>
    [BsonElement("profilePictureUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Embedded artist statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public ArtistStats Stats { get; set; } = new();

    /// <summary>
    /// List of genres associated with this artist (denormalized for performance)
    /// </summary>
    [BsonElement("genres")]
    public List<string> Genres { get; set; } = new();

    /// <summary>
    /// Updates artist statistics
    /// </summary>
    /// <param name="albumsCount">Number of albums</param>
    /// <param name="tracksCount">Number of tracks</param>
    /// <param name="postsCount">Number of posts about this artist</param>
    /// <param name="scratchesCount">Number of scratches about this artist</param>
    public void UpdateStats(int? albumsCount = null, int? tracksCount = null, 
        int? postsCount = null, int? scratchesCount = null)
    {
        if (albumsCount.HasValue) Stats.AlbumsCount = albumsCount.Value;
        if (tracksCount.HasValue) Stats.TracksCount = tracksCount.Value;
        if (postsCount.HasValue) Stats.PostsCount = postsCount.Value;
        if (scratchesCount.HasValue) Stats.ScratchesCount = scratchesCount.Value;

        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a genre to the artist
    /// </summary>
    /// <param name="genre">Genre name to add</param>
    public void AddGenre(string genre)
    {
        if (!string.IsNullOrWhiteSpace(genre) && !Genres.Contains(genre, StringComparer.OrdinalIgnoreCase))
        {
            Genres.Add(genre);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a genre from the artist
    /// </summary>
    /// <param name="genre">Genre name to remove</param>
    public void RemoveGenre(string genre)
    {
        var existingGenre = Genres.FirstOrDefault(g => string.Equals(g, genre, StringComparison.OrdinalIgnoreCase));
        if (existingGenre != null)
        {
            Genres.Remove(existingGenre);
            MarkAsUpdated();
        }
    }
}
