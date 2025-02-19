

using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetByQueryAsync(string query, int limit);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByFirebaseIdAsync(string id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<List<User>> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task<UserProfileDto> GetUserProfileByIdAsync(int userId, int currentUserId);
    }
}
