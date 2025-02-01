using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IScratchService
    {
        Task<Scratch> CreateNewAsync(CreateScratchRequestDto newScratch, User currUser);
        Task<bool> DeleteScratchByIdAsync(string id);
        Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId);
        Task<ScratchDetailsResponseDto> GetDetailsById(string scratchId);
        //Task<IEnumerable<Scratch>> GetHomeFeedByUserIdAsync(int userId);
        Task<IEnumerable<Scratch>> GetHomeFeedByUserIdListAsync(List<int> homeFeedUserIds);
        Task<bool> SetUserImageURLOnScratch(Scratch entity, string result);
    }
}
