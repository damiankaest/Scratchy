using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly IScratchRepository _scratchRepository;

        public CollectionService(IScratchRepository scratchRepository) {
            _scratchRepository = scratchRepository;
        }

        public async Task<CollectionResponseDto> GetCollectionByUserId(string userId)
        {
            var scratches = await _scratchRepository.GetByUserIdAsync(userId);
            var result = new CollectionResponseDto();
            result.Albums =
            [
                .. scratches
                .GroupBy(s => s.Album.AlbumId)
                .Select(g => new CollectionItem
                {
                    Id = g.Key,
                    Artist = g.First().Album.Artist.Name,
                    CoverUri = g.First().Album.CoverImageUrl,
                    Rating = g.Average(s => s.Rating),
                    Title = g.First().Album.Title,
                })
                .ToList()
,
            ];

            return result;
        }
    }
}
