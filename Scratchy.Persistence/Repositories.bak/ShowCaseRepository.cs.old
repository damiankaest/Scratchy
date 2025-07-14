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
        public async Task<ShowCase> AddAsync(ShowCase entity)
        {
            await _context.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(id);
        }

        public Task<IEnumerable<ShowCase>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ShowCase> GetByIdAsync(int id)
        {
            return await _context.ShowCases.FirstAsync(x=>x.Id == id);
        }

        public async Task<IEnumerable<ShowCase>> GetByUserId(int userId) => await _context.ShowCases.Where(x => x.UserId == userId).ToListAsync();

        public async Task UpdateAsync(ShowCase entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
