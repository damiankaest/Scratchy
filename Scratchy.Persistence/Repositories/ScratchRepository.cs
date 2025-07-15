using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories;

/// <summary>
/// MongoDB repository implementation for Scratch operations
/// </summary>
public class ScratchRepository : MongoRepository<ScratchDocument>, IScratchRepository
{
    private readonly ILogger<ScratchRepository> _logger;
    private readonly IMongoRepository<UserDocument> _userRepository;
    private readonly IMongoRepository<AlbumDocument> _albumRepository;
    private readonly IMongoRepository<TrackDocument> _trackRepository;

    public ScratchRepository(
        MongoDbContext context,
        ILogger<ScratchRepository> logger,
        IMongoRepository<UserDocument> userRepository,
        IMongoRepository<AlbumDocument> albumRepository,
        IMongoRepository<TrackDocument> trackRepository)
        : base(context, logger)
    {
        _logger = logger;
        _userRepository = userRepository;
        _albumRepository = albumRepository;
        _trackRepository = trackRepository;
    }

    public Task<IEnumerable<ScratchDocument>> GetByUserAndAlbumIdIdAsync(string userId, string albumId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ScratchDocument>> GetByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ScratchDocument>> GetScratchesAsync(List<string> userIdList)
    {
        throw new NotImplementedException();
    }

    public Task<CreateScratchResponseDto> UploadAsync(CreateScratchRequestDto newScratch)
    {
        throw new NotImplementedException();
    }
}
