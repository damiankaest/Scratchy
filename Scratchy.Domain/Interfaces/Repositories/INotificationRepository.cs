using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification); // Eine neue Benachrichtigung hinzufügen
        Task<Notification> GetByIdAsync(int id); // Eine Benachrichtigung anhand ihrer ID abrufen
        Task<List<Notification>> GetByUserIdAsync(string userId); // Alle Benachrichtigungen eines Nutzers abrufen
        Task UpdateAsync(Notification notification); // Eine Benachrichtigung aktualisieren
        Task DeleteAsync(int id); // Eine Benachrichtigung löschen
        Task MarkAsReadAsync(int id); // Eine Benachrichtigung als gelesen markieren
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId); // Ungelesene Benachrichtigungen eines Nutzers abrufen
    }
}
