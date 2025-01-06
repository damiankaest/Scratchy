using Scratchy.Domain.DTO.Response.Explore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<IEnumerable<Artist>> GetAllAsync();

        /// <summary>
        /// Holt einen Künstler basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Künstlers.</param>
        /// <returns>Der gefundene Künstler.</returns>
        Task<Artist> GetByIdAsync(int id);

        /// <summary>
        /// Fügt einen neuen Künstler hinzu.
        /// </summary>
        /// <param name="artist">Das Künstlerobjekt.</param>
        /// <returns>Der hinzugefügte Künstler.</returns>
        Task<Artist> AddAsync(Artist artist);

        /// <summary>
        /// Aktualisiert einen bestehenden Künstler.
        /// </summary>
        /// <param name="artist">Das aktualisierte Künstlerobjekt.</param>
        /// <returns>Der aktualisierte Künstler.</returns>
        Task<Artist> UpdateAsync(Artist artist);

        /// <summary>
        /// Löscht einen Künstler basierend auf der ID.
        /// </summary>
        /// <param name="id">Die ID des Künstlers.</param>
        Task DeleteAsync(int id);
    }
}
