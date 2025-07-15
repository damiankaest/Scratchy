using Amazon.Runtime.Internal.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Enum;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Scratchy.Persistence.Repositories
{
    public class UserBadgeRepository : MongoRepository<BadgeDocument>, IUserBadgeRepository
    {
        public UserBadgeRepository(MongoDbContext context, ILogger<UserBadgeRepository> logger ): base( context , logger)
        {
                
        }
        public Task AddAsync(UserBadge userBadge)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> AggregateAsync<TResult>(PipelineDefinition<BadgeDocument, TResult> pipeline, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> AggregateSingleAsync<TResult>(PipelineDefinition<BadgeDocument, TResult> pipeline, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(Expression<Func<BadgeDocument, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument> CreateAsync(BadgeDocument document, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> CreateManyAsync(IEnumerable<BadgeDocument> documents, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int userBadgeId)
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

        public Task<long> DeleteManyAsync(Expression<Func<BadgeDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<BadgeDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> FindAsync(Expression<Func<BadgeDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> FindAsync(FilterDefinition<BadgeDocument> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument?> FindOneAsync(Expression<Func<BadgeDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument?> FindOneAsync(FilterDefinition<BadgeDocument> filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserBadge> GetByUserAndBadgeAsync(int userId, int badgeId)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<BadgeDocument> GetCollection()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeDocument>> GetPagedAsync(Expression<Func<BadgeDocument, bool>>? filter = null, Expression<Func<BadgeDocument, object>>? sortBy = null, SortOrder sortOrder = SortOrder.Ascending, int skip = 0, int limit = 50, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BulkWriteResult<BadgeDocument>> ReplaceManyAsync(IEnumerable<BadgeDocument> documents, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserBadge userBadge)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument> UpdateAsync(BadgeDocument document, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateManyAsync(Expression<Func<BadgeDocument, bool>> filter, UpdateDefinition<BadgeDocument> update, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument?> UpdatePartialAsync(string id, UpdateDefinition<BadgeDocument> update, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDocument> UpsertAsync(Expression<Func<BadgeDocument, bool>> filter, BadgeDocument document, CancellationToken cancellationToken = default)
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
