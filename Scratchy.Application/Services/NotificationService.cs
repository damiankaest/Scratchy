
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> CreateNewAsync(NewMessageDto message)
        {
            //await _notificationRepository.AddAsync(new Notification(message));
            return true;
        }

        public async Task<List<NotificationDocument>> GetNotificationsForUser(string userId)
        {
            var result = await _notificationRepository.GetByUserIdAsync(userId);
            return result.ToList();
        }
    }
}
