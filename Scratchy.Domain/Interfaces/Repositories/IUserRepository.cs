using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IMongoRepository<UserDocument>
    {
        Task<IEnumerable<UserDocument>> GetByQueryAsync(string query, int limit);
        //Task<IEnumerable<UserDocument>> GetAllAsync();
        //Task<UserDocument> GetByFirebaseIdAsync(string id);
        //Task<UserDocument> AddAsync(UserDocument user);
        //Task<UserDocument> UpdateAsync(UserDocument user);
        //Task DeleteAsync(string id);

        //Task<List<UserDocument>> GetByUsernameAsync(string username);
        //Task<UserDocument> GetByEmailAsync(string email);
        //Task<UserDocument> GetByIdAsync(string id);
        //Task<UserProfileDto> GetUserProfileByIdAsync(string userId, string currentUserId);
    }
}
