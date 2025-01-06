using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Scratchy.Domain.DB
{
    public class LibraryEntry
    { 

        public LibraryEntry(Album albumInfo, Scratch newPost)
        {

            Id = ObjectId.GenerateNewId().ToString();
            UserId = newPost.UserId;
            AlbumId = newPost.AlbumId;
            Rating = newPost.Rating;
            PostedAt = DateTime.UtcNow;
        }

        [BsonId]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        [JsonPropertyName("albumId")]
        public string AlbumId { get; set; } = string.Empty;
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        [JsonPropertyName("postedAt")]
        public DateTime PostedAt { get; set; }
    }
}
