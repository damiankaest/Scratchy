using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepository;

        public FollowService(IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }

        public async Task FollowUserAsync(string followerId, string followingId)
        {
            if (followerId == followingId)
                throw new InvalidOperationException("You cannot follow yourself.");

            if (await _followRepository.IsFollowingAsync(followerId, followingId))
                throw new InvalidOperationException("You are already following this user.");

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId,
                CreatedAt = DateTime.UtcNow
            };

            await _followRepository.AddAsync(follow);
        }

        public async Task UnfollowUserAsync(string followerId, string followingId)
        {
            var follow = await _followRepository.GetFollowAsync(followerId, followingId);
            if (follow == null)
                throw new InvalidOperationException("You are not following this user.");

            await _followRepository.RemoveAsync(follow);
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _followRepository.IsFollowingAsync(followerId, followingId);
        }

        public async Task<List<string>> GetFollowersAsync(string userId)
        {
            return await _followRepository.GetFollowersAsync(userId);
        }

        public async Task<List<string>> GetFollowingAsync(string userId)
        {
            return await _followRepository.GetFollowingAsync(userId);
        }
    }

}
