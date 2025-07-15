using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories;

/// <summary>
/// MongoDB repository implementation for Post operations
/// </summary>
public class PostRepository : MongoRepository<PostDocument>, IPostRepository
{
    private readonly ILogger<PostRepository> _logger;

    public PostRepository(
        MongoDbContext context,
        ILogger<PostRepository> logger) 
        : base(context, logger)
    {
        _logger = logger;
    }

    public Task<List<PostDocument>> GetAllByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
