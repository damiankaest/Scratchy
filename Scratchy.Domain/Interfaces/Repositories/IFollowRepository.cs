using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFollowRepository
    {
        Task AddAsync(Follow follow);
        Task RemoveAsync(Follow follow);
        Task<Follow> GetFollowAsync(string followerId, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowersAsync(string userId);
        Task<List<string>> GetFollowingAsync(string userId);
    }

}
