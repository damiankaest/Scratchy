using MongoDB.Bson;
using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly SpotifyApiClient _spotifyApiClient;
        
        public SpotifyService()
        {
            var spotifyAccessToken = SpotifyApiHelper.GetSpotifyAccessToken("2783d9fdaf5848ab9a4b7248215f5844", "02d9765ad365414097f1ed7924f71241").Result;
            _spotifyApiClient = new SpotifyApiClient(spotifyAccessToken);

        }
        public async Task<List<Album>> SearchForAlbumByQuery(string query)
        {
            var albums = _spotifyApiClient.SearchSpotifyAlbumsByQueryAsync(query).Result;
            return albums;
        }
    }
}
