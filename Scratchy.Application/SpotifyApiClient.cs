using Newtonsoft.Json.Linq;
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Application
{
    public class SpotifyApiClient
    {
        private readonly HttpClient _httpClient;
        public SpotifyApiClient(string accessToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<JArray> SearchSpotifyAlbumsByArtistIdAsync(string artistId, int limit = 50)
        {
            string endpoint = $"https://api.spotify.com/v1/artists/{artistId}/albums";
            string requestUri = $"{endpoint}?limit={limit}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(content);
                return (JArray)jsonResponse["items"];
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<JArray> SearchSpotifyArtistsAsync(string query, int limit = 10)
        {
            string endpoint = "https://api.spotify.com/v1/search";
            string requestUri = $"{endpoint}?q={Uri.EscapeDataString(query)}&type=artist&limit={limit}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(content);
                return (JArray)jsonResponse["artists"]["items"];
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<List<Album>> SearchSpotifyAlbumsByQueryAsync(string query, int limit = 10, string market = "DE")
        {
            // Aufbau der Anfrage-URL für die allgemeine Suche
            string requestUri = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=album&limit={limit}&market={market}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Fehler: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(content);

            var albumItems = jsonResponse["albums"]?["items"];
            var albums = new List<Album>();

            // Cache, um bereits abgefragte Artist-Details nicht mehrfach anzufordern
            var artistCache = new Dictionary<string, Artist>();

            if (albumItems != null)
            {
                foreach (var item in albumItems)
                {
                    // Album-Mapping
                    var album = new Album
                    {
                        Title = item["name"]?.ToString(),
                        SpotifyId = item["id"]?.ToString(),
                        ReleaseDate = DateTime.TryParse(item["release_date"]?.ToString(), out var releaseDate)
                                        ? releaseDate
                                        : (DateTime?)null,
                        CoverImageUrl = item["images"]?.FirstOrDefault()?["url"]?.ToString(),
                        CreatedAt = DateTime.Now,
                        Tracks = new List<Track>(),
                        Genres = new List<Genre>(),
                        Posts = new List<Post>(),
                        Scratches = new List<Scratch>()
                    };

                    // Artist-Mapping (hier wird nur der erste Artist aus der Liste verwendet)
                    var artistToken = item["artists"]?.FirstOrDefault();
                    if (artistToken != null)
                    {
                        string artistName = artistToken["name"]?.ToString();
                        string artistSpotifyId = artistToken["id"]?.ToString();

                        Artist artist = null;
                        if (!string.IsNullOrEmpty(artistSpotifyId))
                        {
                            // Prüfe, ob wir die Artist-Details bereits abgefragt haben
                            if (artistCache.ContainsKey(artistSpotifyId))
                            {
                                artist = artistCache[artistSpotifyId];
                            }
                            else
                            {
                                // Zusätzliche Artist-Details von Spotify abrufen
                                string artistRequestUri = $"https://api.spotify.com/v1/artists/{artistSpotifyId}";
                                HttpResponseMessage artistResponse = await _httpClient.GetAsync(artistRequestUri);

                                if (artistResponse.IsSuccessStatusCode)
                                {
                                    string artistContent = await artistResponse.Content.ReadAsStringAsync();
                                    JObject artistJson = JObject.Parse(artistContent);

                                    // Profilbild-URL: Das erste Bild in der Liste verwenden
                                    string profilePictureUrl = artistJson["images"]?.FirstOrDefault()?["url"]?.ToString();

                                    // Genres abrufen und daraus eine Bio generieren, z. B. "Genres: Rock, Pop"
                                    var genresArray = artistJson["genres"]?.ToObject<List<string>>();
                                    string bio = genresArray != null && genresArray.Any()
                                        ? "Genres: " + string.Join(", ", genresArray)
                                        : "No genres available";

                                    artist = new Artist
                                    {
                                        Name = artistName,
                                        SpotifyId = artistSpotifyId,
                                        ProfilePictureUrl = profilePictureUrl,
                                        Bio = bio,
                                        CreatedAt = DateTime.Now
                                    };

                                    // Im Cache ablegen, damit bei mehrfach vorkommendem Artist kein weiterer Aufruf erfolgt
                                    artistCache[artistSpotifyId] = artist;
                                }
                                else
                                {
                                    // Fallback: Minimaler Artist, falls die zusätzliche Abfrage fehlschlägt
                                    artist = new Artist
                                    {
                                        Name = artistName,
                                        SpotifyId = artistSpotifyId,
                                        CreatedAt = DateTime.Now
                                    };
                                }
                            }

                            album.Artist = artist;
                        }
                    }

                    albums.Add(album);
                }
            }

            return albums;
        }
    }
}
