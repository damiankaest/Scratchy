namespace Scratchy.Domain.Interfaces.Services
{
    public interface IFollowerService
    {
        Task FollowUserAsync(string followerId, string followingId);
        Task UnfollowUserAsync(string followerIdstring, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowersAsync(string userId);
        Task<List<string>> GetFollowingAsync(string userId);
    }
}
