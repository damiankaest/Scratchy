
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserBadgeRepository : IMongoRepository<BadgeDocument>
    {
        //Task<UserBadge> GetByUserAndBadgeAsync(int userId, int badgeId);
        //Task AddAsync(UserBadge userBadge);
        //Task UpdateAsync(UserBadge userBadge);
        //Task DeleteAsync(int userBadgeId);
        // usw. je nach Bedarf
    }
}
