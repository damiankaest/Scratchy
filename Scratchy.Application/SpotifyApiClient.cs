using Azure.Core;
using Newtonsoft.Json.Linq;
using Scratchy.Domain.DTO.DB;
using System.Net;
using System.Net.Http.Headers;

namespace Scratchy.Application
{
    public class SpotifyApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _clientId;
        private readonly string _clientSecret;

        private string _accessToken;
        private DateTime _accessTokenExpiration = DateTime.MinValue;

        public SpotifyApiClient(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;

            _httpClient = new HttpClient();
        }


        private async Task EnsureTokenAsync()
        {
            // If current time is greater than or about to be the expiration, refresh
            if (DateTime.UtcNow >= _accessTokenExpiration)
            {
                // Get new token from Spotify
                var tokenResponse = await SpotifyApiHelper.GetSpotifyAccessTokenAsync(_clientId, _clientSecret);

                _accessToken = tokenResponse.AccessToken;
                // "expires_in" is in seconds, so add to current time
                _accessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }

        public async Task<JArray> SearchSpotifyAlbumsByArtistIdAsync(string artistId, int limit = 50)
        {
            await EnsureTokenAsync(); // Ensure token is valid before making the request

            string endpoint = $"https://api.spotify.com/v1/artists/{artistId}/albums";
            string requestUri = $"{endpoint}?limit={limit}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            // If you want, also handle 401 here in case your token is somehow invalidated
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Force refresh
                _accessTokenExpiration = DateTime.MinValue;
                await EnsureTokenAsync();
                // Retry once
                response = await _httpClient.GetAsync(requestUri);
            }

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(content);
            return (JArray)jsonResponse["items"];
        }

        public async Task<JArray> SearchSpotifyArtistsAsync(string query, int limit = 10)
        {
            await EnsureTokenAsync();

            string endpoint = "https://api.spotify.com/v1/search";
            string requestUri = $"{endpoint}?q={Uri.EscapeDataString(query)}&type=artist&limit={limit}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            // Optionally handle 401 again:
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _accessTokenExpiration = DateTime.MinValue;
                await EnsureTokenAsync();
                response = await _httpClient.GetAsync(requestUri);
            }

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(content);
            return (JArray)jsonResponse["artists"]["items"];
        }

        public async Task<List<Album>> SearchSpotifyAlbumsByQueryAsync(string query, int limit = 10, string market = "DE")
        {
            // 1) Make sure we have a valid token before starting
            await EnsureTokenAsync();

            // Build the general search request
            string requestUri = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=album&limit={limit}&market={market}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            // 2) If token was invalid (expired / revoked), we may get a 401
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Force refresh
                await ForceTokenRefreshAsync();
                // Retry once
                response = await _httpClient.GetAsync(requestUri);
            }

            // 3) At this point, if we still aren't successful, throw an error
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Fehler: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(content);

            var albumItems = jsonResponse["albums"]?["items"];
            var albums = new List<Album>();

            // Cache for Artist details
            var artistCache = new Dictionary<string, Artist>();

            if (albumItems != null)
            {
                foreach (var item in albumItems)
                {
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

                    // Map Artist (only first artist, as in your original code)
                    var artistToken = item["artists"]?.FirstOrDefault();
                    if (artistToken != null)
                    {
                        string artistName = artistToken["name"]?.ToString();
                        string artistSpotifyId = artistToken["id"]?.ToString();

                        if (!string.IsNullOrEmpty(artistSpotifyId))
                        {
                            Artist artist;
                            if (artistCache.ContainsKey(artistSpotifyId))
                            {
                                // Already fetched this artist
                                artist = artistCache[artistSpotifyId];
                            }
                            else
                            {
                                // 4) Since we are making another Spotify request, ensure token is valid again
                                await EnsureTokenAsync();

                                string artistRequestUri = $"https://api.spotify.com/v1/artists/{artistSpotifyId}";
                                HttpResponseMessage artistResponse = await _httpClient.GetAsync(artistRequestUri);

                                // Handle a 401 if it occurs
                                if (artistResponse.StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    await ForceTokenRefreshAsync();
                                    artistResponse = await _httpClient.GetAsync(artistRequestUri);
                                }

                                if (artistResponse.IsSuccessStatusCode)
                                {
                                    string artistContent = await artistResponse.Content.ReadAsStringAsync();
                                    JObject artistJson = JObject.Parse(artistContent);

                                    // Use the first image if present
                                    string profilePictureUrl = artistJson["images"]?.FirstOrDefault()?["url"]?.ToString();

                                    // Build a simple "bio" from genre list
                                    var genresArray = artistJson["genres"]?.ToObject<List<string>>();
                                    string bio = (genresArray != null && genresArray.Any())
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
                                }
                                else
                                {
                                    // Minimal fallback if the additional request fails
                                    artist = new Artist
                                    {
                                        Name = artistName,
                                        SpotifyId = artistSpotifyId,
                                        CreatedAt = DateTime.Now
                                    };
                                }
                                // Cache it
                                artistCache[artistSpotifyId] = artist;
                            }
                            album.Artist = artist;
                        }
                    }
                    albums.Add(album);
                }
            }
            return albums;
        }

        private async Task ForceTokenRefreshAsync()
        {
            // Force the expiration time to past -> triggers a new token retrieval
            _accessTokenExpiration = DateTime.MinValue;
            await EnsureTokenAsync();
        }
    }
}
