using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ScratchItDbContext _context;

        public TrackRepository(ScratchItDbContext context)
        {
            _context = context;
        }
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

        public async Task<Track> GetByIdAsync(int id) => await _context.Tracks.FirstOrDefaultAsync(x=>x.TrackId == id);

        public Task UpdateAsync(Track entity)
        {
            throw new NotImplementedException();
        }
    }
}
