
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserBadgeRepository
    {
        Task<UserBadge> GetByUserAndBadgeAsync(int userId, int badgeId);
        Task AddAsync(UserBadge userBadge);
        Task UpdateAsync(UserBadge userBadge);
        Task DeleteAsync(int userBadgeId);
        // usw. je nach Bedarf
    }
}
