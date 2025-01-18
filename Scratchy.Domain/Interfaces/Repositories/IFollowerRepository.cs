using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFollowerRepository
    {
        Task AddAsync(Follow follow);
        Task RemoveAsync(Follow follow);
        Task<Follow> GetFollowAsync(int followerId, int followingId);
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<List<int>> GetFollowersAsync(int userId);
        Task<List<int>> GetFollowingAsync(int userId);
    }

}
