using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository _followerRepository;

        public FollowerService(IFollowerRepository followRepository)
        {
            _followerRepository = followRepository;
        }

        public async Task FollowUserAsync(int followerId, int followingId)
        {
            if (followerId == followingId)
                throw new InvalidOperationException("You cannot follow yourself.");

            if (await _followerRepository.IsFollowingAsync(followerId, followingId))
                throw new InvalidOperationException("You are already following this user.");

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowedId = followingId,
                CreatedAt = DateTime.UtcNow
            };

            await _followerRepository.AddAsync(follow);
        }

        public async Task UnfollowUserAsync(int followerId, int followingId)
        {
            var follow = await _followerRepository.GetFollowAsync(followerId, followingId);
            if (follow == null)
                throw new InvalidOperationException("You are not following this user.");

            await _followerRepository.RemoveAsync(follow);
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
        {
            return await _followerRepository.IsFollowingAsync(followerId, followingId);
        }

        public async Task<List<int>> GetFollowersAsync(int userId)
        {
            return await _followerRepository.GetFollowersAsync(userId);
        }

        public async Task<List<int>> GetFollowingAsync(int userId)
        {
            return await _followerRepository.GetFollowingAsync(userId);
        }

        Task<List<string>> IFollowerService.GetFollowersAsync(int userId)
        {
            throw new NotImplementedException();
        }

        async Task<List<int>> IFollowerService.GetFollowingAsync(int userId)
        {
            var followingIds = await _followerRepository.GetFollowingAsync(userId);
            return followingIds;
        }
    }

}
