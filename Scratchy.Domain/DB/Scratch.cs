using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.DTO;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DB
{
    public class Scratch
    {
        public Scratch()
        {
            
        }

        public Scratch(CreateScratchRequestDto newScratch)
        {
            Id = Guid.NewGuid().ToString();
            UserId = newScratch.UserId;
            Rating = newScratch.Rating;
            AlbumId = newScratch.AlbumId;
            CreatedOn = DateTime.UtcNow;
            UserImageBase64String = newScratch.UserImageAsBase64String;
        }

        public Scratch(CreateScratchRequestDto createPost, Album albumInfo, string userImageUrl)
        {
            Id = Guid.NewGuid().ToString();
            UserId = createPost.UserId;
            Rating = createPost.Rating;
            AlbumName = albumInfo.Name;
            ArtistName = albumInfo.Artist;
            AlbumImageUrl = albumInfo.SpotifyImageUrl;
            SpotifyRefUrl = albumInfo.SpotifyUrl;
            AlbumId = albumInfo.Id;
            CreatedOn = DateTime.UtcNow;
            UserImageUrl = userImageUrl;
        }

        [Key]
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("user_id")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("userName")]
        public string UserName { get; set; } = string.Empty;

        [BsonElement("album_id")]
        public string AlbumId { get; set; } = string.Empty;

        [BsonElement("albumName")]
        public string AlbumName { get; set; } = string.Empty;

        [BsonElement("artistName")]
        public string ArtistName { get; set; } = string.Empty;

        [BsonElement("rating")]
        public double Rating { get; set; } = 0;

        [BsonElement("likeCount")]
        public int LikeCount { get; set; } = 0;

        [BsonElement("likedBy")]
        public List<string> LikedBy { get; set; } = new List<string>();

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; } = new List<Comment>();

        [BsonElement("userImageUrl")]
        public string UserImageUrl { get; set; } = string.Empty;

        [BsonElement("albumImageUrl")]
        public string AlbumImageUrl { get; set; } = string.Empty;

        [BsonElement("spotifyRefUrl")]
        public string SpotifyRefUrl { get; set; } = string.Empty;
        [BsonElement("createdOn")]
        public DateTime CreatedOn { get; set; }
        [BsonElement("userImageBase64String")]
        public object UserImageBase64String { get; }
    }
}
