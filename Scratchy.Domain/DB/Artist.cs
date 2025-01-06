using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;

public class Artist
{
    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("genres")]
    public List<string> Genres { get; set; }
    [BsonElement("spotifyUrl")]
    public string SpotifyUrl { get; set; }
    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; }

    public Artist()
    {
        // Parameterloser Standardkonstruktor
    }

    public Artist(JObject artistJObject)
    {
        if (artistJObject == null)
            throw new ArgumentNullException(nameof(artistJObject));

        Id = artistJObject["id"]?.ToString();
        Name = artistJObject["name"]?.ToString();

        // Genres ist ein Array von Strings
        Genres = artistJObject["genres"]?.Select(g => g.ToString()).ToList() ?? new List<string>();

        // Spotify-URL aus external_urls
        SpotifyUrl = artistJObject["external_urls"]?["spotify"]?.ToString();

        // ImageUrl aus dem ersten verfügbaren Bild
        var images = artistJObject["images"] as JArray;
        if (images != null && images.Any())
        {
            // Du kannst auch nach einer bestimmten Größe filtern
            var image = images.First;
            ImageUrl = image["url"]?.ToString();
        }
        else
        {
            ImageUrl = null;
        }
    }
}
