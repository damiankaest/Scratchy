using MongoDB.Bson;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly SpotifyApiClient _spotifyApiClient;
        
        public SpotifyService(string clientId, string clientSecret)
        {
            var spotifyAccessToken = SpotifyApiHelper.GetSpotifyAccessTokenAsync("2783d9fdaf5848ab9a4b7248215f5844", "02d9765ad365414097f1ed7924f71241").Result;
            _spotifyApiClient = new SpotifyApiClient(clientId, clientSecret);

        }
        public async Task<List<Album>> SearchForAlbumByQuery(string query, int limit)
        {
            // Implementation depends on your own data model
            // E.g., adapt JArray to List<Album>
            var albumsJson = await _spotifyApiClient.SearchSpotifyAlbumsByQueryAsync(query, limit);
            List<Album> albums = albumsJson;

            return albums;
        }
    }
}
