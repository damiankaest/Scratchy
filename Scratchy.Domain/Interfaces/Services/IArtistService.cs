using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IArtistService
    {
        /// <summary>
        /// Holt Künstler basierend auf einer Suchabfrage.
        /// </summary>
        /// <param name="query">Die Suchabfrage.</param>
        /// <param name="limit">Maximale Anzahl von Ergebnissen.</param>
        /// <returns>Eine Liste von Künstlern.</returns>
        Task<IEnumerable<ExploreArtistsDto>> GetByQueryAsync(string query, int limit);

        /// <summary>
        /// Holt alle Künstler.
        /// </summary>
        /// <returns>Eine Liste von Künstlern.</returns>
        Task<IEnumerable<ArtistDocument>> GetAllAsync();

        /// <summary>
        /// Holt einen Künstler basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Künstlers.</param>
        /// <returns>Der gefundene Künstler.</returns>
        Task<ArtistDocument> GetByIdAsync(string id);

        /// <summary>
        /// Fügt einen neuen Künstler hinzu.
        /// </summary>
        /// <param name="artist">Das Künstlerobjekt.</param>
        /// <returns>Der hinzugefügte Künstler.</returns>
        Task<ArtistDocument> AddAsync(ArtistDocument artist);

        /// <summary>
        /// Aktualisiert einen bestehenden Künstler.
        /// </summary>
        /// <param name="artist">Das aktualisierte Künstlerobjekt.</param>
        /// <returns>Der aktualisierte Künstler.</returns>
        Task<ArtistDocument> UpdateAsync(ArtistDocument artist);

        /// <summary>
        /// Löscht einen Künstler basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Künstlers.</param>
        Task DeleteAsync(string id);
    }
}
