using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IAlbumRepository : IRepository<Album>
    {
        /// <summary>
        /// Sucht Alben basierend auf einer Abfrage.
        /// </summary>
        /// <param name="query">Die Suchanfrage.</param>
        /// <returns>Eine Liste von Alben, die der Abfrage entsprechen.</returns>
        Task<List<Album>> GetByQueryAsync(string query, int limit = 10);

        /// <summary>
        /// Ruft ein Album basierend auf seiner Spotify-ID ab.
        /// </summary>
        /// <param name="spotifyId">Die Spotify-ID des Albums.</param>
        /// <returns>Das entsprechende Album oder null, wenn keines gefunden wurde.</returns>
        Task<Album> GetBySpotifyIdAsync(string spotifyId);
        Task<IEnumerable<Album>> GetByListOfIdAsync(List<int> ids);
    }
}
