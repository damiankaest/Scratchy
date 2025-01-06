using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification); // Eine neue Benachrichtigung hinzufügen
        Task<Notification> GetByIdAsync(string id); // Eine Benachrichtigung anhand ihrer ID abrufen
        Task<List<Notification>> GetByUserIdAsync(string userId); // Alle Benachrichtigungen eines Nutzers abrufen
        Task UpdateAsync(Notification notification); // Eine Benachrichtigung aktualisieren
        Task DeleteAsync(string id); // Eine Benachrichtigung löschen
        Task MarkAsReadAsync(string id); // Eine Benachrichtigung als gelesen markieren
        Task<List<Notification>> GetUnreadNotificationsAsync(string userId); // Ungelesene Benachrichtigungen eines Nutzers abrufen
    }
}
