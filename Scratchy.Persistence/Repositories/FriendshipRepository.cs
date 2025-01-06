using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ScratchItDbContext _context;

        public FriendshipRepository(ScratchItDbContext context)
        {
            _context = context;
        }
        public object GetByUserId(string requestId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Friendship>> GetByUserIdAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId))
                .ToListAsync();
        }

        public async Task AddAsync(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Friendship friendship)
        {
            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetFriendIdsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
                .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
                .ToListAsync();
        }

        public Task<Friendship> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
