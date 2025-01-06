using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        object GetByUserId(string requestId);

        Task<Friendship> GetByIdAsync(string id);
        Task<List<Friendship>> GetByUserIdAsync(string userId);
        Task AddAsync(Friendship friendship);
        Task UpdateAsync(Friendship friendship);
        Task<List<string>> GetFriendIdsAsync(string userId);
    }
}
