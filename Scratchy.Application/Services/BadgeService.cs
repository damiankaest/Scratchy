using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;

        public BadgeService(IBadgeRepository badgeRepository,
                            IUserBadgeRepository userBadgeRepository)
        {
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
        }

        // --- CRUD für BADGES ---
        public async Task<IEnumerable<Badge>> GetAllBadgesAsync()
        {
            return await _badgeRepository.GetAllAsync();
        }

        public async Task<Badge> GetBadgeByIdAsync(string badgeId)
        {
            return await _badgeRepository.GetByIdAsync(badgeId);
        }

        public async Task<Badge> CreateBadgeAsync(Badge badge)
        {
            await _badgeRepository.AddAsync(badge);
            return badge;
        }

        public async Task UpdateBadgeAsync(Badge badge)
        {
            await _badgeRepository.UpdateAsync(badge);
        }

        public async Task DeleteBadgeAsync(string badgeId)
        {
            // ggf. erst prüfen, ob der Badge genutzt wird (UserBadge-Einträge?)
            await _badgeRepository.DeleteAsync(badgeId);
        }

        // --- BADGE-Funktionen für USER ---
        public async Task AddBadgeToUserAsync(string userId, string badgeId)
        {
            // Prüfen, ob UserBadge schon existiert:
            var existing = await _userBadgeRepository.GetByUserAndBadgeAsync(userId, badgeId);
            if (existing != null)
            {
                // ggf. Level erhöhen oder eine Exception werfen,
                // je nachdem wie du dein Business-Case definierst
                existing.Level++;
                await _userBadgeRepository.UpdateAsync(existing);
                return;
            }

            // Sonst neuen Eintrag anlegen:
            var userBadge = new UserBadge
            {
                UserId = userId,
                BadgeId = badgeId,
                Level = 1,
                EarnedOn = DateTime.Now
            };
            await _userBadgeRepository.AddAsync(userBadge);
        }

        public async Task IncrementUserBadgeLevelAsync(string userId, string badgeId)
        {
            var userBadge = await _userBadgeRepository.GetByUserAndBadgeAsync(userId, badgeId);
            if (userBadge != null)
            {
                userBadge.Level++;
                await _userBadgeRepository.UpdateAsync(userBadge);
            }
            else
            {
                // Badge existiert beim User gar nicht -> ggf. Exception
                // oder automatisch hinzufügen:
                // await AddBadgeToUserAsync(userId, badgeId);
            }
        }

        public object GetDisplayedBadgesByUserId(string? currentUserId)
        {
            throw new NotImplementedException();
        }

        public object GetByUserId(string? currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
