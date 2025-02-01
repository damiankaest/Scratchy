using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class ShowCaseRepository : IShowCaseRepository
    {
        private readonly ScratchItDbContext _context;

        public ShowCaseRepository(ScratchItDbContext context)
        {
            _context = context;  
        }
        public Task<ShowCase> AddAsync(ShowCase entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShowCase>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShowCase> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ShowCase>> GetByUserId(int userId)
        {
            var userShowCases = await _context.ShowCases.Where(x => x.UserId == userId).ToListAsync();
            return userShowCases;
        }


        public Task UpdateAsync(ShowCase entity)
        {
            throw new NotImplementedException();
        }
    }
}
