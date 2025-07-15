using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IScratchRepository : IMongoRepository<ScratchDocument>
    {
        Task<IEnumerable<ScratchDocument>> GetByUserAndAlbumIdIdAsync(string userId, string albumId);
        Task<IEnumerable<ScratchDocument>> GetByUserIdAsync(string userId);
        Task<IEnumerable<ScratchDocument>> GetScratchesAsync(List<string> userIdList);
        Task<CreateScratchResponseDto> UploadAsync(CreateScratchRequestDto newScratch);
    }
}
