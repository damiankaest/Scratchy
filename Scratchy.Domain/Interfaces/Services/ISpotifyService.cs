using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ISpotifyService
    {
        public Task<List<Album>> SearchForAlbumByQuery(string query);
    }
}
