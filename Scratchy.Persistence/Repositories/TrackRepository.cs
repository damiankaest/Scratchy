using Microsoft.Extensions.Logging;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;
using Scratchy.Persistence.Repositories;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public class TrackRepository : MongoRepository<TrackDocument>, ITrackRepository
    {
        public TrackRepository(MongoDbContext context, ILogger<TrackRepository> logger) : base(context, logger)
        {
            
        }
    }
}
