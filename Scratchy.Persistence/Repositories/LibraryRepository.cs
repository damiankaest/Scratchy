using MongoDB.Driver;

using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ScratchItDbContext _context;

        public LibraryRepository(ScratchItDbContext context) {
        _context = context;
        }

        public Task AddAsync(LibraryEntry entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryEntry>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LibraryEntry> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LibraryEntry> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryEntry>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LibraryEntry entity)
        {
            throw new NotImplementedException();
        }
    }
}
