using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserBadgeRepository
    {
        Task<UserBadge> GetByUserAndBadgeAsync(string userId, string badgeId);
        Task AddAsync(UserBadge userBadge);
        Task UpdateAsync(UserBadge userBadge);
        Task DeleteAsync(string userBadgeId);
        // usw. je nach Bedarf
    }
}
