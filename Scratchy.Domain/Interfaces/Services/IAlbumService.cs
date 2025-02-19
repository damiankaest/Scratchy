
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IAlbumService
    {
        Task<List<ExploreAlbumDto>> GetAlbumExploreInfoAsync(string query, int limit);
        Task<Album> GetByIdAsync(int albumId);
        Task<List<NewScratchAlbumSearchResponseDto>> GetByQueryAsync(string query);
        Task<AlbumDetailsDto> GetDetailsByIdAsync(int albumId);
    }
}
