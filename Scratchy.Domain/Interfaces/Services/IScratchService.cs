using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IScratchService
    {
        Task<ScratchDocument> CreateNewAsync(CreateScratchRequestDto newScratch, UserDocument currUser);
        Task<bool> DeleteScratchByIdAsync(string id);
        Task<List<CollectionAlbumScratchesDto>> GetAlbumScratchesForUserAsync(string albumId, string userId);
        Task<IEnumerable<ScratchDocument>> GetByUserIdAsync(string userId);
        Task<ScratchDetailsResponseDto> GetDetailsById(string scratchId);
        Task<IEnumerable<ScratchDocument>> GetHomeFeedByUserIdListAsync(List<string> homeFeedUserIds);
        Task<List<AlbumShowCaseEntity>> GetIndividualAlbumsByUserIdAsync(string userId);
        Task<bool> SetUserImageURLOnScratch(ScratchDocument entity, string result);
    }
}
