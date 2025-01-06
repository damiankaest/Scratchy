using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetByQueryAsync(string query, int limit);
        Task<IEnumerable<Artist>> GetAllAsync();
        Task<Artist> GetByIdAsync(int id);
        Task<Artist> AddAsync(Artist artist);
        Task<Artist> UpdateAsync(Artist artist);
        Task DeleteAsync(int id);
    }
}
