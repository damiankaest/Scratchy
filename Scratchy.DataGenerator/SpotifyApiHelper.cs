using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.DataGenerator
{
    public class SpotifyApiHelper
    {
        public static async Task<string> GetSpotifyAccessToken(string clientId, string clientSecret)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
                var clientCredentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", clientCredentials);
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var tokenData = JObject.Parse(json);

                return tokenData["access_token"]?.ToString();
            }
        }
    }
}
