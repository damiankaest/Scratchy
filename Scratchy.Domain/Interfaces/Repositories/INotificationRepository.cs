using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IMongoRepository<NotificationDocument>
    {
        Task AddAsync(NotificationDocument notification); // Eine neue Benachrichtigung hinzufügen
        Task<NotificationDocument> GetByIdAsync(int id); // Eine Benachrichtigung anhand ihrer ID abrufen
        Task<List<NotificationDocument>> GetByUserIdAsync(string userId); // Alle Benachrichtigungen eines Nutzers abrufen
        Task UpdateAsync(NotificationDocument notification); // Eine Benachrichtigung aktualisieren
        Task DeleteAsync(int id); // Eine Benachrichtigung löschen
        Task MarkAsReadAsync(int id); // Eine Benachrichtigung als gelesen markieren
        Task<List<NotificationDocument>> GetUnreadNotificationsAsync(int userId); // Ungelesene Benachrichtigungen eines Nutzers abrufen
    }
}
