using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<AlbumDocument>> SearchAlbumsAsync(string query);
        Task<IEnumerable<ArtistDocument>> SearchArtistsAsync(string query);
        Task<IEnumerable<UserDocument>> SearchUsersAsync(string query);
        Task<(IEnumerable<AlbumDocument> Albums, IEnumerable<ArtistDocument> Artists, IEnumerable<UserDocument> Users)> SearchAllAsync(string query);
    }
}
