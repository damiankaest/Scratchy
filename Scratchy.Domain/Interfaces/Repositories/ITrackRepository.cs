using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface ITrackRepository : IMongoRepository<TrackDocument>
    {
        public Task<Track> AddAsync(TrackDocument entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrackDocument>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TrackDocument> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TrackDocument entity)
        {
            throw new NotImplementedException();
        }
    }
}
