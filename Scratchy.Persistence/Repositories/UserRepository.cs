using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ScratchItDbContext _context;

        public UserRepository(ScratchItDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetByQueryAsync(string query, int limit)
        {
            return await _context.Users
                .Where(u => EF.Functions.Like(u.Username, $"{query}%"))
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<User>> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }


    }
}
