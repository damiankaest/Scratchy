using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFollowerRepository : IMongoRepository<FollowDocument>
    {
        Task AddAsync(FollowDocument follow);
        Task RemoveAsync(FollowDocument follow);
        Task<FollowDocument> GetFollowAsync(string followerIdstring, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowersAsync(string userId);
        Task<List<string>> GetFollowingAsync(string userId);
    }

}
