using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.Enum;
using Scratchy.Domain.Models;
using System.Linq.Expressions;

namespace Scratchy.Domain.Interfaces.Repositories;

/// <summary>
/// Generic MongoDB repository interface for document operations
/// </summary>
/// <typeparam name="T">Document type that inherits from BaseDocument</typeparam>
public interface IMongoRepository<T> where T : BaseDocument
{
    #region Basic CRUD Operations

    /// <summary>
    /// Gets a document by ObjectId
    /// </summary>
    /// <param name="id">Document ObjectId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document or null if not found</returns>
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a document by ObjectId
    /// </summary>
    /// <param name="id">Document ObjectId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document or null if not found</returns>
    Task<T?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all documents
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of documents</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new document
    /// </summary>
    /// <param name="document">Document to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created document with generated Id</returns>
    Task<T> CreateAsync(T document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document
    /// </summary>
    /// <param name="document">Document to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated document</returns>
    Task<T> UpdateAsync(T document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document by ObjectId
    /// </summary>
    /// <param name="id">Document ObjectId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document by ObjectId
    /// </summary>
    /// <param name="id">Document ObjectId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);

    #endregion

    #region Query Operations

    /// <summary>
    /// Finds documents matching the specified filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching documents</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds documents matching the specified filter definition
    /// </summary>
    /// <param name="filter">MongoDB filter definition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching documents</returns>
    Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the first document matching the specified filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>First matching document or null</returns>
    Task<T?> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the first document matching the specified filter definition
    /// </summary>
    /// <param name="filter">MongoDB filter definition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>First matching document or null</returns>
    Task<T?> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<T?> GetByFireBaseId(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets documents with pagination support
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="sortBy">Sort expression</param>
    /// <param name="sortOrder">Sort order (ascending/descending)</param>
    /// <param name="skip">Number of documents to skip</param>
    /// <param name="limit">Number of documents to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated collection of documents</returns>
    Task<IEnumerable<T>> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? sortBy = null,
        SortOrder sortOrder = SortOrder.Ascending,
        int skip = 0,
        int limit = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts documents matching the specified filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of matching documents</returns>
    Task<long> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if any document matches the specified filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if any document matches</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations

    /// <summary>
    /// Creates multiple documents in a single operation
    /// </summary>
    /// <param name="documents">Documents to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created documents with generated Ids</returns>
    Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple documents matching the filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="update">Update definition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of updated documents</returns>
    Task<long> UpdateManyAsync(
        Expression<Func<T, bool>> filter,
        UpdateDefinition<T> update,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes multiple documents matching the filter
    /// </summary>
    /// <param name="filter">Filter expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of deleted documents</returns>
    Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces multiple documents in a single bulk operation
    /// </summary>
    /// <param name="documents">Documents to replace</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Bulk write result</returns>
    Task<BulkWriteResult<T>> ReplaceManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default);

    #endregion

    #region Aggregation Pipeline

    /// <summary>
    /// Executes an aggregation pipeline
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="pipeline">Aggregation pipeline</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Aggregation results</returns>
    Task<IEnumerable<TResult>> AggregateAsync<TResult>(
        PipelineDefinition<T, TResult> pipeline,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an aggregation pipeline and returns a single result
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="pipeline">Aggregation pipeline</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Single aggregation result or default</returns>
    Task<TResult?> AggregateSingleAsync<TResult>(
        PipelineDefinition<T, TResult> pipeline,
        CancellationToken cancellationToken = default);

    #endregion

    #region Transaction Support

    /// <summary>
    /// Executes multiple operations within a transaction
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="operations">Operations to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction result</returns>
    Task<TResult> WithTransactionAsync<TResult>(
        Func<IClientSessionHandle, Task<TResult>> operations,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes multiple operations within a transaction
    /// </summary>
    /// <param name="operations">Operations to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task WithTransactionAsync(
        Func<IClientSessionHandle, Task> operations,
        CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Operations

    /// <summary>
    /// Creates or updates a document (upsert operation)
    /// </summary>
    /// <param name="filter">Filter to find existing document</param>
    /// <param name="document">Document to create or update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upserted document</returns>
    Task<T> UpsertAsync(
        Expression<Func<T, bool>> filter,
        T document,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a partial update on a document
    /// </summary>
    /// <param name="id">Document ObjectId</param>
    /// <param name="update">Update definition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated document or null if not found</returns>
    Task<T?> UpdatePartialAsync(
        string id,
        UpdateDefinition<T> update,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the underlying MongoDB collection for advanced operations
    /// </summary>
    /// <returns>MongoDB collection</returns>
    IMongoCollection<T> GetCollection();

    #endregion
}
