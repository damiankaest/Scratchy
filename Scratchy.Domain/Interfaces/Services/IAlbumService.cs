
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IAlbumService
    {
        Task<List<ExploreAlbumDto>> GetAlbumExploreInfoAsync(string query, int limit);
        Task<AlbumDocument> GetByIdAsync(string albumId);
        Task<List<NewScratchAlbumSearchResponseDto>> GetByQueryAsync(string query);
        Task<AlbumDocument> GetDetailsByIdAsync(string albumId);
    }
}
