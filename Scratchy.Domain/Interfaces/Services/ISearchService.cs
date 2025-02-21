using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Album>> SearchAlbumsAsync(string query);
        Task<IEnumerable<Artist>> SearchArtistsAsync(string query);
        Task<IEnumerable<User>> SearchUsersAsync(string query);
        Task<(IEnumerable<Album> Albums, IEnumerable<Artist> Artists, IEnumerable<User> Users)> SearchAllAsync(string query);
    }
}
