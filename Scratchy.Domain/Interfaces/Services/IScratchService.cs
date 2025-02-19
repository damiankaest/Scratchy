using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IScratchService
    {
        Task<Scratch> CreateNewAsync(CreateScratchRequestDto newScratch, User currUser);
        Task<bool> DeleteScratchByIdAsync(string id);
        Task<List<CollectionAlbumScratchesDto>> GetAlbumScratchesForUserAsync(int albumId, int userId);
        Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId);
        Task<ScratchDetailsResponseDto> GetDetailsById(int scratchId);
        Task<IEnumerable<Scratch>> GetHomeFeedByUserIdListAsync(List<int> homeFeedUserIds);
        Task<List<AlbumShowCaseEntity>> GetIndividualAlbumsByUserIdAsync(int userId);
        Task<bool> SetUserImageURLOnScratch(Scratch entity, string result);
    }
}
