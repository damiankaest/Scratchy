using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IScratchService
    {
        Task<bool> CreateNewAsync(CreateScratchRequestDto newScratch);
        Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId);
        Task GetDetailsById(string scratchId);
        Task<IEnumerable<Scratch>> GetHomeFeedByUserIdAsync(int userId);
    }
}
