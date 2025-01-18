using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        object GetByUserId(string requestId);

        Task<Follow> GetByIdAsync(string id);
        Task<List<Follow>> GetByUserIdAsync(string userId);
        Task AddAsync(Follow friendship);
        Task UpdateAsync(Follow friendship);
        Task<List<string>> GetFriendIdsAsync(string userId);
    }
}
