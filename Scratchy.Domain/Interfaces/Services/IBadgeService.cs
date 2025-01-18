
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IBadgeService
    {
        // --- CRUD für BADGES ---
        Task<IEnumerable<Badge>> GetAllBadgesAsync();
        Task<Badge> GetBadgeByIdAsync(int badgeId);
        Task<Badge> CreateBadgeAsync(Badge badge);
        Task UpdateBadgeAsync(Badge badge);
        Task DeleteBadgeAsync(int badgeId);

        // --- BADGE-Funktionen für USER ---
        Task AddBadgeToUserAsync(int userId, int badgeId);
        Task IncrementUserBadgeLevelAsync(int userId, int badgeId);
        object GetDisplayedBadgesByUserId(int? currentUserId);
        object GetByUserId(int? currentUserId);
        // ggf. weitere Methoden: GetAllBadgesForUserAsync, RemoveBadgeFromUserAsync, usw.
    }
}
