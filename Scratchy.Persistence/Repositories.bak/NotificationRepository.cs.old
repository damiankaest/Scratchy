using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ScratchItDbContext _context;

        public NotificationRepository(ScratchItDbContext context) 
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.User.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await UpdateAsync(notification);
            }
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
