using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IFriendshipRepository : IMongoRepository<FollowDocument>
    {
        object GetByUserId(string requestId);

        Task<FollowDocument> GetByIdAsync(string id);
        Task<List<FollowDocument>> GetByUserIdAsync(string userId);
        Task AddAsync(FollowDocument friendship);
        Task UpdateAsync(FollowDocument friendship);
        Task<List<string>> GetFriendIdsAsync(string userId);
    }
}
