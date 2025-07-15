
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IBadgeService
    {
        // --- CRUD für BADGES ---
        Task<IEnumerable<BadgeDocument>> GetAllBadgesAsync();
        Task<BadgeDocument> GetBadgeByIdAsync(string badgeId);
        Task<BadgeDocument> CreateBadgeAsync(BadgeDocument badge);
        Task UpdateBadgeAsync(BadgeDocument badge);
        Task DeleteBadgeAsync(string badgeId);

        // --- BADGE-Funktionen für USER ---
        Task AddBadgeToUserAsync(string userId, string badgeId);
        Task IncrementUserBadgeLevelAsync(string userId);
        object GetDisplayedBadgesByUserId(string? currentUserId);
        object GetByUserId(string? currentUserId);
        // ggf. weitere Methoden: GetAllBadgesForUserAsync, RemoveBadgeFromUserAsync, usw.
    }
}
