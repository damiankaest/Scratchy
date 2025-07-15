using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Models;

/// <summary>
/// Track reference embedded document for playlists
/// </summary>
[BsonIgnoreExtraElements]
public class PlaylistTrack
{
    [BsonElement("trackId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TrackId { get; set; } = string.Empty;

    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [BsonElement("duration")]
    [BsonIgnoreIfNull]
    public TimeSpan? Duration { get; set; }

    [BsonElement("spotifyId")]
    [StringLength(100)]
    [BsonIgnoreIfNull]
    public string? SpotifyId { get; set; }

    [BsonElement("album")]
    public AlbumReference Album { get; set; } = new();

    [BsonElement("addedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("addedByUserId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? AddedByUserId { get; set; }

    [BsonElement("position")]
    public int Position { get; set; }
}

/// <summary>
/// Playlist statistics embedded document
/// </summary>
[BsonIgnoreExtraElements]
public class PlaylistStats
{
    [BsonElement("tracksCount")]
    [BsonDefaultValue(0)]
    public int TracksCount { get; set; } = 0;

    [BsonElement("totalDuration")]
    [BsonIgnoreIfNull]
    public TimeSpan? TotalDuration { get; set; }

    [BsonElement("followersCount")]
    [BsonDefaultValue(0)]
    public int FollowersCount { get; set; } = 0;

    [BsonElement("playsCount")]
    [BsonDefaultValue(0)]
    public int PlaysCount { get; set; } = 0;
}

/// <summary>
/// Main Playlist document model for MongoDB
/// Collection: playlists
/// </summary>
[BsonIgnoreExtraElements]
public class PlaylistDocument : SoftDeleteDocument
{
    /// <summary>
    /// Playlist title
    /// </summary>
    [BsonElement("title")]
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Playlist description
    /// </summary>
    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string? Description { get; set; }

    /// <summary>
    /// Playlist owner (denormalized user reference)
    /// </summary>
    [BsonElement("owner")]
    public UserReference Owner { get; set; } = new();

    /// <summary>
    /// Playlist statistics (computed/cached fields)
    /// </summary>
    [BsonElement("stats")]
    public PlaylistStats Stats { get; set; } = new();

    /// <summary>
    /// Embedded tracks array (M:M relationship embedded)
    /// </summary>
    [BsonElement("tracks")]
    public List<PlaylistTrack> Tracks { get; set; } = new();

    /// <summary>
    /// Playlist visibility (public, private, followers)
    /// </summary>
    [BsonElement("visibility")]
    [StringLength(20)]
    [BsonDefaultValue("public")]
    public string Visibility { get; set; } = "public";

    /// <summary>
    /// Playlist cover image URL
    /// </summary>
    [BsonElement("coverImageUrl")]
    [StringLength(255)]
    [BsonIgnoreIfNull]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Collaborative playlist flag
    /// </summary>
    [BsonElement("isCollaborative")]
    [BsonDefaultValue(false)]
    public bool IsCollaborative { get; set; } = false;

    /// <summary>
    /// Playlist tags/genres
    /// </summary>
    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Collaborative users (if collaborative playlist)
    /// </summary>
    [BsonElement("collaborators")]
    public List<UserReference> Collaborators { get; set; } = new();

    /// <summary>
    /// Adds a track to the playlist
    /// </summary>
    /// <param name="trackId">Track ID</param>
    /// <param name="title">Track title</param>
    /// <param name="duration">Track duration</param>
    /// <param name="albumId">Album ID</param>
    /// <param name="albumTitle">Album title</param>
    /// <param name="artistName">Artist name</param>
    /// <param name="addedByUserId">User who added the track</param>
    /// <param name="spotifyId">Spotify track ID</param>
    /// <returns>The created playlist track</returns>
    public PlaylistTrack AddTrack(string trackId, string title, TimeSpan? duration = null,
        string? albumId = null, string? albumTitle = null, string? artistName = null,
        string? addedByUserId = null, string? spotifyId = null)
    {
        var playlistTrack = new PlaylistTrack
        {
            TrackId = trackId,
            Title = title,
            Duration = duration,
            SpotifyId = spotifyId,
            AddedByUserId = addedByUserId,
            Position = Tracks.Count + 1,
            Album = new AlbumReference
            {
                AlbumId = albumId ?? string.Empty,
                Title = albumTitle ?? string.Empty,
                Artist = new ArtistReference
                {
                    Name = artistName ?? string.Empty
                }
            }
        };

        Tracks.Add(playlistTrack);
        RecalculateStats();
        MarkAsUpdated();

        return playlistTrack;
    }

    /// <summary>
    /// Removes a track from the playlist
    /// </summary>
    /// <param name="trackId">Track ID to remove</param>
    public void RemoveTrack(string trackId)
    {
        var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
        if (track != null)
        {
            Tracks.Remove(track);
            ReorderTracks();
            RecalculateStats();
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Reorders tracks and updates positions
    /// </summary>
    /// <param name="trackId">Track ID to move</param>
    /// <param name="newPosition">New position</param>
    public void ReorderTrack(string trackId, int newPosition)
    {
        var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
        if (track != null && newPosition > 0 && newPosition <= Tracks.Count)
        {
            Tracks.Remove(track);
            Tracks.Insert(newPosition - 1, track);
            ReorderTracks();
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Sets the playlist owner
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void SetOwner(string userId, string username, string? profilePictureUrl = null)
    {
        Owner = new UserReference
        {
            UserId = userId,
            Username = username,
            ProfilePictureUrl = profilePictureUrl
        };
        MarkAsUpdated();
    }

    /// <summary>
    /// Adds a collaborator to the playlist
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="username">Username</param>
    /// <param name="profilePictureUrl">Profile picture URL</param>
    public void AddCollaborator(string userId, string username, string? profilePictureUrl = null)
    {
        if (!Collaborators.Any(c => c.UserId == userId))
        {
            Collaborators.Add(new UserReference
            {
                UserId = userId,
                Username = username,
                ProfilePictureUrl = profilePictureUrl
            });
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Removes a collaborator from the playlist
    /// </summary>
    /// <param name="userId">User ID to remove</param>
    public void RemoveCollaborator(string userId)
    {
        var collaborator = Collaborators.FirstOrDefault(c => c.UserId == userId);
        if (collaborator != null)
        {
            Collaborators.Remove(collaborator);
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// Adds a tag to the playlist
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
    /// Updates playlist statistics
    /// </summary>
    /// <param name="followersCount">Number of followers</param>
    /// <param name="playsCount">Number of plays</param>
    public void UpdateStats(int? followersCount = null, int? playsCount = null)
    {
        if (followersCount.HasValue) Stats.FollowersCount = followersCount.Value;
        if (playsCount.HasValue) Stats.PlaysCount = playsCount.Value;

        RecalculateStats();
        MarkAsUpdated();
    }

    /// <summary>
    /// Recalculates computed statistics
    /// </summary>
    private void RecalculateStats()
    {
        Stats.TracksCount = Tracks.Count;
        Stats.TotalDuration = Tracks.Where(t => t.Duration.HasValue)
                                    .Aggregate(TimeSpan.Zero, (total, track) => total + track.Duration!.Value);
    }

    /// <summary>
    /// Reorders all tracks and updates positions
    /// </summary>
    private void ReorderTracks()
    {
        for (int i = 0; i < Tracks.Count; i++)
        {
            Tracks[i].Position = i + 1;
        }
    }
}
