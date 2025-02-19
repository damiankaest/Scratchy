using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ICollectionService
    {
        public Task<CollectionResponseDto> GetCollectionByUserId(int userId);
    }
}
