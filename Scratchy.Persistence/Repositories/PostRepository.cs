using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        public PostRepository(IMongoDatabase database) 
        {
        }

        public Task AddAsync(Post entity)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Post entity)
        {
            try
            {
                var bsonEntity = entity.ToBsonDocument();
                await _collection.InsertOneAsync(bsonEntity);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id)));
        }

        public async Task<List<Post>> GetAllAsync()
        {
            try
            {
                var postList = await _collection.Find(_ => true).ToListAsync();
                var postListSerialized = postList.Select(doc => BsonSerializer.Deserialize<Post>(doc)).ToList();
                return postListSerialized;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<Post>> GetAllByUserIdAsync(string userId)
        {
            var postList = await _collection.Find(Builders<BsonDocument>.Filter.Eq("userId", userId)).ToListAsync();
            var postListSerialized = postList.Select(doc => BsonSerializer.Deserialize<Post>(doc)).ToList();
            return postListSerialized;
        }

        public async Task<Post> GetByIdAsync(string userId)
        {
            var postList = _collection.Find(Builders<BsonDocument>.Filter.Eq("userId", userId)).First();
            var postListSerialized = postList.Select(doc => BsonSerializer.Deserialize<Post>(postList)).First();
            return postListSerialized;
        }

        public Task UpdateAsync(string id, Post entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Post entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Post>> IRepository<Post>.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
