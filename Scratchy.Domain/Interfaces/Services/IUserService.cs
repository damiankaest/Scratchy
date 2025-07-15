using Scratchy.Domain.Models;

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
        Task<IEnumerable<UserDocument>> GetAllAsync();

        /// <summary>
        /// Holt einen Benutzer basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>Der gefundene Benutzer.</returns>
        Task<UserDocument> GetByIdAsync(string id);

        /// <summary>
        /// Fügt einen neuen Benutzer hinzu.
        /// </summary>
        /// <param name="user">Das Benutzerobjekt.</param>
        /// <returns>Der hinzugefügte Benutzer.</returns>
        Task<UserDocument> AddAsync(UserDocument user);

        /// <summary>
        /// Aktualisiert einen bestehenden Benutzer.
        /// </summary>
        /// <param name="user">Das aktualisierte Benutzerobjekt.</param>
        /// <returns>Der aktualisierte Benutzer.</returns>
        Task<UserDocument> UpdateAsync(UserDocument user);

        /// <summary>
        /// Löscht einen Benutzer basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        Task DeleteAsync(int id);
        Task<bool> SendFriendRequest(UserDocument currentUser, UserDocument userResult);
        Task<UserDocument> GetUserByFireBaseId(string currentUserID);
        Task<UserDocument> GetUserProfileByIdAsync( string currentUserId);
    }
}
