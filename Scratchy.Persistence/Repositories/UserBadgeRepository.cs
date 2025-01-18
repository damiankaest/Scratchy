using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{

    public class UserBadgeRepository : IUserBadgeRepository
    {
        private readonly ScratchItDbContext _context;

        public UserBadgeRepository(ScratchItDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Findet einen UserBadge-Eintrag anhand von User und Badge.
        /// </summary>
        /// <param name="userId">ID des Benutzers</param>
        /// <param name="badgeId">ID des Badges</param>
        /// <returns>UserBadge oder null, falls nicht vorhanden</returns>
        public async Task<UserBadge> GetByUserAndBadgeAsync(int userId, int badgeId)
        {
            return await _context.UserBadges
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BadgeId == badgeId);
        }

        /// <summary>
        /// Legt einen neuen Eintrag in der UserBadges-Tabelle an.
        /// </summary>
        /// <param name="userBadge">Das zu erstellende UserBadge-Objekt</param>
        public async Task AddAsync(UserBadge userBadge)
        {
            await _context.UserBadges.AddAsync(userBadge);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Aktualisiert einen bestehenden Eintrag (z. B. Level-Up).
        /// </summary>
        /// <param name="userBadge">Aktualisiertes UserBadge-Objekt</param>
        public async Task UpdateAsync(UserBadge userBadge)
        {
            _context.UserBadges.Update(userBadge);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Löscht einen Eintrag anhand der eindeutigen ID (Primärschlüssel in UserBadges).
        /// </summary>
        /// <param name="userBadgeId">ID des UserBadge-Eintrags</param>
        public async Task DeleteAsync(int userBadgeId)
        {
            var userBadge = await _context.UserBadges.FindAsync(userBadgeId);
            if (userBadge != null)
            {
                _context.UserBadges.Remove(userBadge);
                await _context.SaveChangesAsync();
            }
        }
    }
}
