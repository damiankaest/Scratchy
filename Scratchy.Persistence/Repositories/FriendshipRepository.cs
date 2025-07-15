using Amazon.Runtime.Internal.Util;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Scratchy.Persistence.Repositories
{
    public class FriendshipRepository : MongoRepository<FollowDocument>, IFriendshipRepository
    {
        private readonly ILogger<FriendshipRepository> logger;
        public FriendshipRepository(MongoDbContext context, ILogger<FriendshipRepository> _logger) : base(context,_logger)
        {
            
        }
        public Task AddAsync(FollowDocument friendship)
        {
            throw new NotImplementedException();
        }

        public Task<FollowDocument> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public object GetByUserId(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FollowDocument>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetFriendIdsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FollowDocument friendship)
        {
            throw new NotImplementedException();
        }
    }
}
