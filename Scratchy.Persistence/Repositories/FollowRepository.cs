using Microsoft.Extensions.Logging;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class FollowRepository : MongoRepository<FollowDocument>, IFollowerRepository
    {
        public FollowRepository(MongoDbContext context, ILogger<FollowRepository> logger) : base(context,logger)
        {
            
        }
        public Task AddAsync(FollowDocument follow)
        {
            throw new NotImplementedException();
        }

        public Task<FollowDocument> GetFollowAsync(string followerIdstring, string followingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetFollowersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetFollowingAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(FollowDocument follow)
        {
            throw new NotImplementedException();
        }
    }
}
