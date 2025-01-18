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
            // Verwenden Sie die allgemeine Suche ohne spezielle Filter, um die internen Suchalgorithmen von Spotify zu nutzen
            string requestUri = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=album&limit={limit}&market={market}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Fehler: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(content);

            var albums = jsonResponse["albums"]?["items"]?
                .Select(item => new Album())
                .ToList() ?? new List<Album>();

            return albums;
        }
    }
}
