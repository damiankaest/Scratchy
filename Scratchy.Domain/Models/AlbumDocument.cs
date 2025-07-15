using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Track embedded document for albums
/// </summary>
[BsonIgnoreExtraElements]
public class TrackEmbedded
{
    [BsonElement("trackId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TrackId { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [BsonElement("duration")]
    [BsonIgnoreIfNull]
    public TimeSpan? Duration { get; set; }

    [BsonElement("trackNumber")]
    public int TrackNumber { get; set; }

    [BsonElement("spotifyId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }

    [BsonElement("previewUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? PreviewUrl { get; set; }

    [BsonElement("isrc")]
    [StringLength(50)]
    [BsonIgnoreIfNull]
    public string? Isrc { get; set; }

    [BsonElement("isExplicit")]
    [BsonDefaultValue(false)]
    public bool IsExplicit { get; set; } = false;

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Artist reference embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class ArtistReference
{
    [BsonElement("artistId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ArtistId { get; set; } = string.Empty;

    [BsonElement("name")]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [BsonElement("spotifyId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }
}

/// <summary>
/// Album statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class AlbumStats
{
    [BsonElement("tracksCount")]
    [BsonDefaultValue(0)]
    public int TracksCount { get; set; } = 0;

    [BsonElement("postsCount")]
    [BsonDefaultValue(0)]
    public int PostsCount { get; set; } = 0;

    [BsonElement("scratchesCount")]
    [BsonDefaultValue(0)]
    public int ScratchesCount { get; set; } = 0;

    [BsonElement("averageRating")]
    [BsonIgnoreIfDefault]
    public decimal AverageRating { get; set; } = 0;

    [BsonElement("ratingsCount")]
    [BsonDefaultValue(0)]
    public int RatingsCount { get; set; } = 0;
}

/// <summary>
/// Main Album document model for MongoDB
/// Collection: albums
/// </summary>
[BsonIgnoreExtraElements]
public class AlbumDocument : BaseDocument
{
    /// <summary>
    /// Album title
    /// </summary>
    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Spotify album ID (external reference)
    /// </summary>
    [BsonElement("spotifyId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }

    /// <summary>
    /// Album release date
    /// </summary>
    [BsonElement("releaseDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonIgnoreIfNull]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Album cover image URL
    /// </summary>
    [BsonElement("coverImageUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// NFC tag ID for physical album identification
    /// </summary>
    [BsonElement("nfcTagId")]
    [StringLength(50)]
    [BsonIgnoreIfNull]
    public string? NfcTagId { get; set; }

    /// <summary>
    /// Artist reference (denormalized for performance)
    /// </summary>
    [BsonElement("artist")]
    public ArtistReference Artist { get; set; } = new();

    /// <summary>
    /// Album statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public AlbumStats Stats { get; set; } = new();

    /// <summary>
    /// Embedded tracks array (1:M relationship embedded)
    /// </summary>
    [BsonElement("tracks")]
    public List<TrackEmbedded> Tracks { get; set; } = new();

    /// <summary>
    /// Genres associated with this album
    /// </summary>
    [BsonElement("genres")]
    public List<string> Genres { get; set; } = new();

    /// <summary>
    /// Album type (e.g., "album", "single", "compilation")
    /// </summary>
    [BsonElement("albumType")]
    [StringLength(50)]
    [BsonDefaultValue("album")]
    public string AlbumType { get; set; } = "album";

    /// <summary>
    /// Record label
    /// </summary>
    [BsonElement("label")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? Label { get; set; }

    /// <summary>
    /// Copyright information
    /// </summary>
    [BsonElement("copyright")]
    [BsonIgnoreIfNull]
    public string? Copyright { get; set; }

    /// <summary>
    /// Adds a track to the album
    /// </summary>
    /// <param name="title">Track title</param>
    /// <param name="trackNumber">Track number</param>
    /// <param name="duration">Track duration</param>
    /// <param name="spotifyId">Spotify track ID</param>
    /// <returns>The created track</returns>
    public TrackEmbedded AddTrack(string title, int trackNumber, TimeSpan? duration = null, string? spotifyId = null)
    {
        var track = new TrackEmbedded
        {
            Title = title,
            TrackNumber = trackNumber,
            Duration = duration,
            SpotifyId = spotifyId
        };

        Tracks.Add(track);
        Stats.TracksCount = Tracks.Count;
        MarkAsUpdated();

        return track;
    }

    /// <summary>
    /// Removes a track from the album
    /// </summary>
    /// <param name="trackId">Track ID to remove</param>
    public void RemoveTrack(string trackId)
    {
        var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
        if (track != null)
        {
            Tracks.Remove(track);
            Stats.TracksCount = Tracks.Count;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Updates album statistics
    /// </summary>
    /// <param name="postsCount">Number of posts</param>
    /// <param name="scratchesCount">Number of scratches</param>
    /// <param name="averageRating">Average rating</param>
    /// <param name="ratingsCount">Number of ratings</param>
    public void UpdateStats(int? postsCount = null, int? scratchesCount = null, 
        decimal? averageRating = null, int? ratingsCount = null)
    {
        if (postsCount.HasValue) Stats.PostsCount = postsCount.Value;
        if (scratchesCount.HasValue) Stats.ScratchesCount = scratchesCount.Value;
        if (averageRating.HasValue) Stats.AverageRating = averageRating.Value;
        if (ratingsCount.HasValue) Stats.RatingsCount = ratingsCount.Value;

        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a genre to the album
    /// </summary>
    /// <param name="genre">Genre name</param>
    public void AddGenre(string genre)
    {
        if (!string.IsNullOrWhiteSpace(genre) && !Genres.Contains(genre, StringComparer.OrdinalIgnoreCase))
        {
            Genres.Add(genre);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Sets the artist reference
    /// </summary>
    /// <param name="artistId">Artist ID</param>
    /// <param name="artistName">Artist name</param>
    /// <param name="spotifyId">Artist Spotify ID</param>
    public void SetArtist(string artistId, string artistName, string? spotifyId = null)
    {
        Artist = new ArtistReference
        {
            ArtistId = artistId,
            Name = artistName,
            SpotifyId = spotifyId
        };
        MarkAsUpdated();
    }
}
