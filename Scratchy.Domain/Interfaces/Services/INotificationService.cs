
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task<bool> CreateNewAsync(NewMessageDto message);
        public Task<List<NotificationDocument>> GetNotificationsForUser(string userId);
    }
}
