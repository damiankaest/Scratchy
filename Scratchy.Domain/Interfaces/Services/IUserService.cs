using System.Collections.Generic;
using System.Threading.Tasks;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response.Explore;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Holt Benutzer basierend auf einer Suchabfrage.
        /// </summary>
        /// <param name="query">Die Suchabfrage.</param>
        /// <param name="limit">Maximale Anzahl von Ergebnissen.</param>
        /// <returns>Eine Liste von Benutzern.</returns>
        Task<IEnumerable<ExploreUserDto>> GetByQueryAsync(string query, int limit);

        /// <summary>
        /// Holt alle Benutzer.
        /// </summary>
        /// <returns>Eine Liste von Benutzern.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Holt einen Benutzer basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>Der gefundene Benutzer.</returns>
        Task<User> GetByIdAsync(string id);

        /// <summary>
        /// Fügt einen neuen Benutzer hinzu.
        /// </summary>
        /// <param name="user">Das Benutzerobjekt.</param>
        /// <returns>Der hinzugefügte Benutzer.</returns>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Aktualisiert einen bestehenden Benutzer.
        /// </summary>
        /// <param name="user">Das aktualisierte Benutzerobjekt.</param>
        /// <returns>Der aktualisierte Benutzer.</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Löscht einen Benutzer basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        Task DeleteAsync(int id);
        Task<bool> SendFriendRequest(User currentUser, User userResult);
    }
}
