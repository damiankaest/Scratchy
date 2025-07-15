using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Track document model for MongoDB
/// Collection: tracks
/// </summary>
[BsonIgnoreExtraElements]
public class TrackDocument : BaseDocument
{
    /// <summary>
    /// Track title
    /// </summary>
    [BsonElement("title")]
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Track duration in seconds
    /// </summary>
    [BsonElement("durationSeconds")]
    [BsonDefaultValue(0)]
    public int DurationSeconds { get; set; } = 0;

    /// <summary>
    /// Album ID reference
    /// </summary>
    [BsonElement("albumId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AlbumId { get; set; } = string.Empty;

    /// <summary>
    /// Artist ID reference
    /// </summary>
    [BsonElement("artistId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ArtistId { get; set; } = string.Empty;

    /// <summary>
    /// Track number in the album
    /// </summary>
    [BsonElement("trackNumber")]
    [BsonDefaultValue(1)]
    public int TrackNumber { get; set; } = 1;

    /// <summary>
    /// Spotify track ID
    /// </summary>
    [BsonElement("spotifyId")]
    [StringLength(50)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }

    /// <summary>
    /// Preview URL for the track
    /// </summary>
    [BsonElement("previewUrl")]
    [StringLength(500)]
    [BsonIgnoreIfNull]
    public string? PreviewUrl { get; set; }

    /// <summary>
    /// Track audio features (embedded document)
    /// </summary>
    [BsonElement("audioFeatures")]
    [BsonIgnoreIfNull]
    public TrackAudioFeatures? AudioFeatures { get; set; }

    /// <summary>
    /// Genres associated with this track
    /// </summary>
    [BsonElement("genres")]
    public List<string> Genres { get; set; } = new();

    /// <summary>
    /// Explicit content flag
    /// </summary>
    [BsonElement("isExplicit")]
    [BsonDefaultValue(false)]
    public bool IsExplicit { get; set; } = false;

    /// <summary>
    /// Track popularity score (0-100)
    /// </summary>
    [BsonElement("popularity")]
    [BsonDefaultValue(0)]
    public int Popularity { get; set; } = 0;

    /// <summary>
    /// External URLs (Spotify, YouTube, etc.)
    /// </summary>
    [BsonElement("externalUrls")]
    public Dictionary<string, string> ExternalUrls { get; set; } = new();
}

/// <summary>
/// Track audio features embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class TrackAudioFeatures
{
    [BsonElement("acousticness")]
    [BsonDefaultValue(0.0)]
    public double Acousticness { get; set; } = 0.0;

    [BsonElement("danceability")]
    [BsonDefaultValue(0.0)]
    public double Danceability { get; set; } = 0.0;

    [BsonElement("energy")]
    [BsonDefaultValue(0.0)]
    public double Energy { get; set; } = 0.0;

    [BsonElement("instrumentalness")]
    [BsonDefaultValue(0.0)]
    public double Instrumentalness { get; set; } = 0.0;

    [BsonElement("liveness")]
    [BsonDefaultValue(0.0)]
    public double Liveness { get; set; } = 0.0;

    [BsonElement("loudness")]
    [BsonDefaultValue(0.0)]
    public double Loudness { get; set; } = 0.0;

    [BsonElement("speechiness")]
    [BsonDefaultValue(0.0)]
    public double Speechiness { get; set; } = 0.0;

    [BsonElement("tempo")]
    [BsonDefaultValue(0.0)]
    public double Tempo { get; set; } = 0.0;

    [BsonElement("valence")]
    [BsonDefaultValue(0.0)]
    public double Valence { get; set; } = 0.0;

    [BsonElement("key")]
    [BsonDefaultValue(-1)]
    public int Key { get; set; } = -1;

    [BsonElement("mode")]
    [BsonDefaultValue(-1)]
    public int Mode { get; set; } = -1;

    [BsonElement("timeSignature")]
    [BsonDefaultValue(4)]
    public int TimeSignature { get; set; } = 4;
}
