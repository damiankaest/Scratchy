using Microsoft.Extensions.Logging;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories;

/// <summary>
/// MongoDB repository implementation for Album operations
/// </summary>
public class AlbumRepository : MongoRepository<AlbumDocument>, IAlbumRepository
{
    private readonly ILogger<AlbumRepository> _logger;
    private readonly IMongoRepository<ArtistDocument> _artistRepository;
    private readonly IMongoRepository<TrackDocument> _trackRepository;
    private readonly IMongoRepository<ScratchDocument> _scratchRepository;

    public AlbumRepository(
        MongoDbContext context,
        ILogger<AlbumRepository> logger,
        IMongoRepository<ArtistDocument> artistRepository,
        IMongoRepository<TrackDocument> trackRepository,
        IMongoRepository<ScratchDocument> scratchRepository)
        : base(context, logger)
    {
        _logger = logger;
        _artistRepository = artistRepository;
        _trackRepository = trackRepository;
        _scratchRepository = scratchRepository;
    }

    public Task<List<AlbumDocument>> GetByQueryAsync(string query, int limit = 10)
    {
        throw new NotImplementedException();
    }
}
