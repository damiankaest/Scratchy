
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

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
        public async Task<IEnumerable<BadgeDocument>> GetAllBadgesAsync()
        {
            return await _badgeRepository.GetAllAsync();
        }

        public async Task<BadgeDocument> GetBadgeByIdAsync(string badgeId)
        {
            return await _badgeRepository.GetByIdAsync(badgeId);
        }

        public async Task<BadgeDocument> CreateBadgeAsync(BadgeDocument badge)
        {
            await _badgeRepository.CreateAsync(badge);
            return badge;
        }

        public async Task UpdateBadgeAsync(BadgeDocument badge)
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
            var existing = await _userBadgeRepository.GetByIdAsync(userId);
            if (existing != null)
            {
                // ggf. Level erhöhen oder eine Exception werfen,
                // je nachdem wie du dein Business-Case definierst
                await _userBadgeRepository.UpdateAsync(existing);
                return;
            }

            // Sonst neuen Eintrag anlegen:
            var userBadge = new BadgeDocument
            {
                
            };
            await _userBadgeRepository.CreateAsync(userBadge);
        }

        public async Task IncrementUserBadgeLevelAsync(string userId)
        {
            //TODO FIX BADGE
            var userBadge = await _userBadgeRepository.GetByIdAsync(userId);
            if (userBadge != null)
            {
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

        public Task DeleteBadgeAsync(int badgeId)
        {
            throw new NotImplementedException();
        }

        public object GetDisplayedBadgesByUserId(int? currentUserId)
        {
            throw new NotImplementedException();
        }

        public object GetByUserId(int? currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
