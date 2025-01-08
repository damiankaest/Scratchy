using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IBadgeService
    {
        // --- CRUD für BADGES ---
        Task<IEnumerable<Badge>> GetAllBadgesAsync();
        Task<Badge> GetBadgeByIdAsync(string badgeId);
        Task<Badge> CreateBadgeAsync(Badge badge);
        Task UpdateBadgeAsync(Badge badge);
        Task DeleteBadgeAsync(string badgeId);

        // --- BADGE-Funktionen für USER ---
        Task AddBadgeToUserAsync(string userId, string badgeId);
        Task IncrementUserBadgeLevelAsync(string userId, string badgeId);
        object GetDisplayedBadgesByUserId(string? currentUserId);
        object GetByUserId(string? currentUserId);
        // ggf. weitere Methoden: GetAllBadgesForUserAsync, RemoveBadgeFromUserAsync, usw.
    }
}
