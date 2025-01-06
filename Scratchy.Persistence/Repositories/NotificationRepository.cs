using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Scratchy.Domain.DB;
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

        public async Task<Notification> GetByIdAsync(string id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(string id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null && !notification.HasSeen)
            {
                notification.HasSeen = true;
                await UpdateAsync(notification);
            }
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverId == userId && !n.HasSeen)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

    }
}
