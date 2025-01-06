using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DB
{
    public class Album
    {
        public Album()
        {

        }
        public Album(JObject albumObject)
        {
            Id = albumObject["id"].ToString();
            SpotifyUrl = albumObject["external_urls"]["spotify"].ToString();
            Name = albumObject["name"].ToString();
            Artist = albumObject["artists"][0]["name"].ToString(); // Assuming you take the first artist
            SpotifyId = albumObject["id"].ToString(); // Same as Id
            SpotifyImageUrl = albumObject["images"][0]["url"].ToString();
        }
        [BsonId]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("artist")]
        public string Artist { get; set; } = string.Empty;
        [BsonElement("spotifyId")]
        public string SpotifyId { get; set; } = string.Empty;
        [BsonElement("spotifyImageUrl")]
        public string SpotifyImageUrl { get; set; } = string.Empty;
        [BsonElement("spotifyUrl")]
        public string SpotifyUrl { get; set; } = string.Empty;
        
    }
}
