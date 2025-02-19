using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface ITrackRepository : IRepository<Track>
    {
        public Task<Track> AddAsync(Track entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Track>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Track> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Track entity)
        {
            throw new NotImplementedException();
        }
    }
}
