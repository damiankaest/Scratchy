using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IScratchRepository : IRepository<Scratch>
    {
        Task<IEnumerable<Scratch>> GetByUserAndAlbumIdIdAsync(int userId, int albumId);
        Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Scratch>> GetScratchesAsync(List<int> userIdList);
        Task<CreateScratchResponseDto> UploadAsync(CreateScratchRequestDto newScratch);
    }
}
