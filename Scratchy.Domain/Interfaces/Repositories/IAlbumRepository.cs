using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IAlbumRepository : IMongoRepository<AlbumDocument>
    {
        /// <summary>
        /// Sucht Alben basierend auf einer Abfrage.
        /// </summary>
        /// <param name="query">Die Suchanfrage.</param>
        /// <returns>Eine Liste von Alben, die der Abfrage entsprechen.</returns>
        Task<List<AlbumDocument>> GetByQueryAsync(string query, int limit = 10);

        /// <summary>
        /// Ruft ein Album basierend auf seiner Spotify-ID ab.
        /// </summary>
        /// <param name="spotifyId">Die Spotify-ID des Albums.</param>
        /// <returns>Das entsprechende Album oder null, wenn keines gefunden wurde.</returns>
        //Task<AlbumDocument> GetBySpotifyIdAsync(string spotifyId);
        //Task<IEnumerable<Album>> GetByListOfIdAsync(List<int> ids);
        //Task<AlbumDocument> GetDetailsByIdAsync(string albumId);
    }
}
