using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.Enum;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;
using System.Linq.Expressions;

namespace Scratchy.Persistence.Repositories;

/// <summary>
/// Base MongoDB repository implementation providing common CRUD operations
/// </summary>
/// <typeparam name="T">Document type that inherits from BaseDocument</typeparam>
public class MongoRepository<T> : IMongoRepository<T> where T : BaseDocument
{
    protected readonly IMongoCollection<T> _collection;
    protected readonly MongoDbContext _context;
    protected readonly ILogger<MongoRepository<T>> _logger;

    public MongoRepository(MongoDbContext context, ILogger<MongoRepository<T>> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _collection = _context.GetCollection<T>();
    }

    #region Basic CRUD Operations

    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                _logger.LogWarning("Invalid ObjectId format: {Id}", id);
                return null;
            }

            return await GetByIdAsync(objectId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document by id: {Id}", id);
            throw;
        }
    }

    public virtual async Task<T?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document by ObjectId: {Id}", id);
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all documents");
            throw;
        }
    }

    public virtual async Task<T> CreateAsync(T document, CancellationToken cancellationToken = default)
    {
        try
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            // Set creation timestamp
            document.CreatedAt = DateTime.UtcNow;
            document.UpdatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(document, null, cancellationToken);
            
            _logger.LogInformation("Created document with id: {Id}", document.Id);
            return document;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating document");
            throw;
        }
    }

    public virtual async Task<T> UpdateAsync(T document, CancellationToken cancellationToken = default)
    {
        try
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (string.IsNullOrEmpty(document.Id))
                throw new ArgumentException("Document Id cannot be null or empty", nameof(document));

            // Update timestamp
            document.UpdatedAt = DateTime.UtcNow;

            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(document.Id));
            var result = await _collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = false }, cancellationToken);

            if (result.MatchedCount == 0)
            {
                _logger.LogWarning("Document not found for update: {Id}", document.Id);
                throw new InvalidOperationException($"Document with id {document.Id} not found");
            }

            _logger.LogInformation("Updated document with id: {Id}", document.Id);
            return document;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating document with id: {Id}", document?.Id);
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                _logger.LogWarning("Invalid ObjectId format for deletion: {Id}", id);
                return false;
            }

            return await DeleteAsync(objectId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document by id: {Id}", id);
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter, cancellationToken);

            var deleted = result.DeletedCount > 0;
            if (deleted)
            {
                _logger.LogInformation("Deleted document with id: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Document not found for deletion: {Id}", id);
            }

            return deleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document by ObjectId: {Id}", id);
            throw;
        }
    }

    #endregion

    #region Query Operations

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding documents with expression filter");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding documents with filter definition");
            throw;
        }
    }

    public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding single document with expression filter");
            throw;
        }
    }

    public virtual async Task<T?> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding single document with filter definition");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? sortBy = null,
        SortOrder sortOrder = SortOrder.Ascending,
        int skip = 0,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var findOptions = new FindOptions<T>
            {
                Skip = skip,
                Limit = limit
            };

            // Apply sorting if specified
            if (sortBy != null)
            {
                findOptions.Sort = sortOrder == SortOrder.Ascending 
                    ? Builders<T>.Sort.Ascending(sortBy)
                    : Builders<T>.Sort.Descending(sortBy);
            }
            else
            {
                // Default sort by creation date descending
                findOptions.Sort = Builders<T>.Sort.Descending(x => x.CreatedAt);
            }

            var filterDefinition = filter != null 
                ? Builders<T>.Filter.Where(filter)
                : FilterDefinition<T>.Empty;

            var cursor = _collection.Find(filterDefinition);
            
            // Apply sorting
            if (sortBy != null)
            {
                cursor = sortOrder == SortOrder.Ascending 
                    ? cursor.Sort(Builders<T>.Sort.Ascending(sortBy))
                    : cursor.Sort(Builders<T>.Sort.Descending(sortBy));
            }
            else
            {
                // Default sort by creation date descending
                cursor = cursor.Sort(Builders<T>.Sort.Descending(x => x.CreatedAt));
            }

            // Apply pagination
            cursor = cursor.Skip(skip).Limit(limit);

            return await cursor.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paged documents. Skip: {Skip}, Limit: {Limit}", skip, limit);
            throw;
        }
    }

    public virtual async Task<long> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var filterDefinition = filter != null 
                ? Builders<T>.Filter.Where(filter)
                : FilterDefinition<T>.Empty;

            return await _collection.CountDocumentsAsync(filterDefinition, null, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting documents");
            throw;
        }
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _collection.CountDocumentsAsync(filter, new CountOptions { Limit = 1 }, cancellationToken);
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking document existence");
            throw;
        }
    }

    #endregion

    #region Bulk Operations

    public virtual async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
    {
        try
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            var documentsList = documents.ToList();
            if (!documentsList.Any())
                return documentsList;

            // Set timestamps for all documents
            var now = DateTime.UtcNow;
            foreach (var document in documentsList)
            {
                document.CreatedAt = now;
                document.UpdatedAt = now;
            }

            await _collection.InsertManyAsync(documentsList, null, cancellationToken);
            
            _logger.LogInformation("Created {Count} documents", documentsList.Count);
            return documentsList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating multiple documents");
            throw;
        }
    }

    public virtual async Task<long> UpdateManyAsync(
        Expression<Func<T, bool>> filter,
        UpdateDefinition<T> update,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Add UpdatedAt to the update definition
            var updateWithTimestamp = update.Set(x => x.UpdatedAt, DateTime.UtcNow);
            
            var result = await _collection.UpdateManyAsync(filter, updateWithTimestamp, null, cancellationToken);
            
            _logger.LogInformation("Updated {Count} documents", result.ModifiedCount);
            return result.ModifiedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating multiple documents");
            throw;
        }
    }

    public virtual async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _collection.DeleteManyAsync(filter, cancellationToken);
            
            _logger.LogInformation("Deleted {Count} documents", result.DeletedCount);
            return result.DeletedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting multiple documents");
            throw;
        }
    }

    public virtual async Task<BulkWriteResult<T>> ReplaceManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
    {
        try
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            var documentsList = documents.ToList();
            if (!documentsList.Any())
                throw new ArgumentException("Documents collection cannot be empty", nameof(documents));

            var bulkOps = new List<WriteModel<T>>();
            var now = DateTime.UtcNow;

            foreach (var document in documentsList)
            {
                if (string.IsNullOrEmpty(document.Id))
                    throw new ArgumentException("All documents must have valid Ids for bulk replace operation");

                document.UpdatedAt = now;
                
                var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(document.Id));
                bulkOps.Add(new ReplaceOneModel<T>(filter, document));
            }

            var result = await _collection.BulkWriteAsync(bulkOps, null, cancellationToken);
            
            _logger.LogInformation("Bulk replaced {Count} documents. Modified: {Modified}", 
                documentsList.Count, result.ModifiedCount);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk replace operation");
            throw;
        }
    }

    #endregion

    #region Aggregation Pipeline

    public virtual async Task<IEnumerable<TResult>> AggregateAsync<TResult>(
        PipelineDefinition<T, TResult> pipeline,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Aggregate(pipeline).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing aggregation pipeline");
            throw;
        }
    }

    public virtual async Task<TResult?> AggregateSingleAsync<TResult>(
        PipelineDefinition<T, TResult> pipeline,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _collection.Aggregate(pipeline).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing aggregation pipeline for single result");
            throw;
        }
    }

    #endregion

    #region Transaction Support

    public virtual async Task<TResult> WithTransactionAsync<TResult>(
        Func<IClientSessionHandle, Task<TResult>> operations,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var session = await _context.Client.StartSessionAsync(cancellationToken: cancellationToken);
            
            return await session.WithTransactionAsync(async (sessionHandle, ct) =>
            {
                return await operations(sessionHandle);
            }, null, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing transaction");
            throw;
        }
    }

    public virtual async Task WithTransactionAsync(
        Func<IClientSessionHandle, Task> operations,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var session = await _context.Client.StartSessionAsync(cancellationToken: cancellationToken);
            
            await session.WithTransactionAsync(async (sessionHandle, ct) =>
            {
                await operations(sessionHandle);
                return true; // Return value required for the transaction method
            }, null, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing transaction");
            throw;
        }
    }

    #endregion

    #region Advanced Operations

    public virtual async Task<T> UpsertAsync(
        Expression<Func<T, bool>> filter,
        T document,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            // Update timestamp
            document.UpdatedAt = DateTime.UtcNow;
            
            // If document doesn't have creation timestamp, set it
            if (document.CreatedAt == default)
                document.CreatedAt = DateTime.UtcNow;

            var options = new ReplaceOptions { IsUpsert = true };
            var result = await _collection.ReplaceOneAsync(filter, document, options, cancellationToken);

            if (result.UpsertedId != null)
            {
                document.Id = result.UpsertedId.AsObjectId.ToString();
                _logger.LogInformation("Upserted new document with id: {Id}", document.Id);
            }
            else
            {
                _logger.LogInformation("Updated existing document with id: {Id}", document.Id);
            }

            return document;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error upserting document");
            throw;
        }
    }

    public virtual async Task<T?> UpdatePartialAsync(
        string id,
        UpdateDefinition<T> update,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                _logger.LogWarning("Invalid ObjectId format for partial update: {Id}", id);
                return null;
            }

            // Add UpdatedAt to the update definition
            var updateWithTimestamp = update.Set(x => x.UpdatedAt, DateTime.UtcNow);
            
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var options = new FindOneAndUpdateOptions<T> { ReturnDocument = ReturnDocument.After };
            
            var result = await _collection.FindOneAndUpdateAsync(filter, updateWithTimestamp, options, cancellationToken);
            
            if (result != null)
            {
                _logger.LogInformation("Partially updated document with id: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Document not found for partial update: {Id}", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error partially updating document with id: {Id}", id);
            throw;
        }
    }

    public virtual IMongoCollection<T> GetCollection()
    {
        return _collection;
    }

    public async Task<T?> GetByFireBaseId(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq("firebaseId", id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document by ObjectId: {Id}", id);
            throw;
        }
    }

    #endregion
}
