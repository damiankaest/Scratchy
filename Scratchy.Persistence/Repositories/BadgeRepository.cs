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
    public class BadgeRepository : MongoRepository<BadgeDocument>, IBadgeRepository
    {
        public BadgeRepository(MongoDbContext context, ILogger<BadgeRepository> logger) : base(context, logger)
        {
            
        }
        public Task AddAsync(BadgeDocument badge)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BadgeDocument badge)
        {
            throw new NotImplementedException();
        }
    }
}
