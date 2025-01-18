namespace Scratchy.Domain.Interfaces.Services
{
    public interface IFollowerService
    {
        Task FollowUserAsync(string followerId, int followingId);
        Task UnfollowUserAsync(int followerIdstring, int followingId);
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<List<string>> GetFollowersAsync(int userId);
        Task<List<string>> GetFollowingAsync(string firebaseId);
    }
}
