using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<List<ExploreAlbumDto>> GetAlbumExploreInfoAsync(string query, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Die Suchabfrage darf nicht leer sein.", nameof(query));
            var albumsExplorer = new List<ExploreAlbumDto>();

            foreach (var album in await _albumRepository.FindAsync(query))
            {
                albumsExplorer.Add(new ExploreAlbumDto(album));
            }

            return albumsExplorer.Take(limit).ToList();
        }

        public async Task<AlbumDocument> GetByIdAsync(string albumId)
        {
            return await _albumRepository.GetByIdAsync(albumId);
        }

        public async Task<List<NewScratchAlbumSearchResponseDto>> GetByQueryAsync(string query)
        {
            var albumCollection = await _albumRepository.FindAsync(query);
            List<NewScratchAlbumSearchResponseDto> albumSearchList = new List<NewScratchAlbumSearchResponseDto>();

            foreach (var item in albumCollection)
            {
                albumSearchList.Add(new NewScratchAlbumSearchResponseDto()
                {
                    AlbumId = item.Id,
                    ArtistName = item.Artist.Name,
                    CoverImageUrl = item.CoverImageUrl,
                    Title = item.Title
                });
            }

            return albumSearchList;
        }

        public async Task<AlbumDocument> GetDetailsByIdAsync(string albumId)
        {
            return await _albumRepository.GetByIdAsync(albumId);
        }
    }
}
