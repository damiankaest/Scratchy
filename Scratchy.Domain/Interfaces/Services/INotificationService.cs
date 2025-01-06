using Scratchy.Domain.DB;
using Scratchy.Domain.DTO.Request;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task<bool> CreateNewAsync(NewMessageDto message);
        public Task<List<Notification>> GetNotificationsForUser(string userId);
    }
}
