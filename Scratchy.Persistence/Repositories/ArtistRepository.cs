using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.Enum;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;
using System.Linq.Expressions;

namespace Scratchy.Persistence.Repositories
{
    public class ArtistRepository : MongoRepository<ArtistDocument>, IArtistRepository
    {
        private readonly ILogger<ArtistRepository> _logger;

        public ArtistRepository(MongoDbContext context, ILogger<ArtistRepository> logger) : base(context,logger)
        {
            
        }

        public Task<ArtistDocument> AddAsync(ArtistDocument artist)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> AggregateAsync<TResult>(PipelineDefinition<ArtistDocument, TResult> pipeline, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> AggregateSingleAsync<TResult>(PipelineDefinition<ArtistDocument, TResult> pipeline, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(Expression<Func<ArtistDocument, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument> CreateAsync(ArtistDocument document, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> CreateManyAsync(IEnumerable<ArtistDocument> documents, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> DeleteManyAsync(Expression<Func<ArtistDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<ArtistDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> FindAsync(Expression<Func<ArtistDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> FindAsync(FilterDefinition<ArtistDocument> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument?> FindOneAsync(Expression<Func<ArtistDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument?> FindOneAsync(FilterDefinition<ArtistDocument> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> GetByQueryAsync(string query, int limit)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<ArtistDocument> GetCollection()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtistDocument>> GetPagedAsync(Expression<Func<ArtistDocument, bool>>? filter = null, Expression<Func<ArtistDocument, object>>? sortBy = null, SortOrder sortOrder = SortOrder.Ascending, int skip = 0, int limit = 50, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BulkWriteResult<ArtistDocument>> ReplaceManyAsync(IEnumerable<ArtistDocument> documents, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument> UpdateAsync(ArtistDocument artist)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument> UpdateAsync(ArtistDocument document, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateManyAsync(Expression<Func<ArtistDocument, bool>> filter, UpdateDefinition<ArtistDocument> update, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument?> UpdatePartialAsync(string id, UpdateDefinition<ArtistDocument> update, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtistDocument> UpsertAsync(Expression<Func<ArtistDocument, bool>> filter, ArtistDocument document, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> WithTransactionAsync<TResult>(Func<IClientSessionHandle, Task<TResult>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WithTransactionAsync(Func<IClientSessionHandle, Task> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
