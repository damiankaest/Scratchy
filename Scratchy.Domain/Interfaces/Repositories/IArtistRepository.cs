using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IArtistRepository : IMongoRepository<ArtistDocument>
    {
        Task<IEnumerable<ArtistDocument>> GetByQueryAsync(string query, int limit);
        //Task<IEnumerable<ArtistDocument>> GetAllAsync();
        //Task<ArtistDocument> GetByIdAsync(int id);
        //Task<ArtistDocument> AddAsync(ArtistDocument artist);
        //Task<ArtistDocument> UpdateAsync(ArtistDocument artist);
        //Task DeleteAsync(int id);
    }
}
