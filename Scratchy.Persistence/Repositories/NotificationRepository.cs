using Microsoft.Extensions.Logging;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class NotificationRepository : MongoRepository<NotificationDocument>, INotificationRepository
    {
        public NotificationRepository(MongoDbContext context, ILogger<NotificationRepository> logger):base(context, logger)
        {
            
        }
        public Task AddAsync(NotificationDocument notification)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDocument> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationDocument>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationDocument>> GetUnreadNotificationsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(NotificationDocument notification)
        {
            throw new NotImplementedException();
        }
    }
}
