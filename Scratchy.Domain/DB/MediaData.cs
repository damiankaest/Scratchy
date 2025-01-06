using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using Scratchy.Domain.DTO;

namespace Scratchy.Domain.DB
{
    public class MediaData
    {
        

        public MediaData()
        {
            
        }
        public MediaData(CreateMediaDataDTO createPost, string artistId)
        {
            id = ObjectId.GenerateNewId().ToString();
            imdbRating = createPost.imdbRating;
            spotifyUrl = createPost.spotifyUrl;
            title = createPost.title;
            description = createPost.description;
            this.artistId = artistId;
        }

        public MediaData(JObject album)
        {
            id = ObjectId.GenerateNewId().ToString();
            title = album["name"]?.ToString() ?? string.Empty;
            spotifyId = album["id"]?.ToString() ?? string.Empty;
            spotifyUrl = album["external_urls"]?["spotify"]?.ToString() ?? string.Empty;
            imgUrl = album["images"]?.FirstOrDefault()?["url"]?.ToString() ?? string.Empty;
            // Get the first artist
            artistId = album["artists"]?.FirstOrDefault()?["id"]?.ToString() ?? string.Empty;
            // Set other fields as needed. For simplicity, imdbRating and description are not set here.
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string artistId { get; set; } = string.Empty;
        public string spotifyId { get; set; } = string.Empty;

        public float imdbRating { get; set; } = 0f;
        public string spotifyUrl { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string imgUrl { get; set; } = string.Empty;
    }
}
