using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Scratchy.Application
{
    public class SpotifyApiHelper
    {
        public static async Task<SpotifyTokenResponse> GetSpotifyAccessTokenAsync(string clientId, string clientSecret)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        });

                // Spotify requires base64-encoded clientId:clientSecret in the Authorization header
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<SpotifyTokenResponse>(jsonResponse);

                return tokenResponse;
            }
        }
    }

    public class SpotifyTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        // Optionally store token type if you like
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
