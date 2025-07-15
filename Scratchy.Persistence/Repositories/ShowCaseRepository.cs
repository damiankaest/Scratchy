using Microsoft.Extensions.Logging;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Persistence.Repositories
{
    public class ShowCaseRepository : MongoRepository<ShowCase>, IShowCaseRepository
    {

        public ShowCaseRepository(MongoDbContext context, ILogger<ShowCaseRepository> logger) : base(context, logger)
        {
            
        }
        public Task<ShowCase> AddAsync(ShowCase entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShowCase>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShowCase> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShowCase>> GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ShowCase entity)
        {
            throw new NotImplementedException();
        }
    }
}
