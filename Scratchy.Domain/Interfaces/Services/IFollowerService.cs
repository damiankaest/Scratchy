namespace Scratchy.Domain.Interfaces.Services
{
    public interface IFollowerService
    {
        Task FollowUserAsync(int followerId, int followingId);
        Task UnfollowUserAsync(int followerIdstring, int followingId);
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<List<string>> GetFollowersAsync(int userId);
        Task<List<int>> GetFollowingAsync(int userId);
    }
}
