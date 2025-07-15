using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Application.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
        }

        public async Task<IEnumerable<ExploreArtistsDto>> GetByQueryAsync(string query, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Die Suchabfrage darf nicht leer sein.", nameof(query));
            var artistExplorer = new List<ExploreArtistsDto>();

            foreach (var artist in await _artistRepository.FindAsync(query))
            {
                artistExplorer.Add(new ExploreArtistsDto(artist));
            }

            return artistExplorer.Take(limit);
        }

        public async Task<IEnumerable<ArtistDocument>> GetAllAsync()
        {
            return await _artistRepository.GetAllAsync();
        }

        public async Task<ArtistDocument> GetByIdAsync(string id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {id} gefunden.");

            return artist;
        }

        public async Task<ArtistDocument> AddAsync(ArtistDocument artist)
        {
            if (artist == null)
                throw new ArgumentNullException(nameof(artist));

            return await _artistRepository.CreateAsync(artist);
        }

        public async Task<ArtistDocument> UpdateAsync(ArtistDocument artist)
        {
            if (artist == null)
                throw new ArgumentNullException(nameof(artist));

            var existingArtist = await _artistRepository.GetByIdAsync(artist.Id);
            if (existingArtist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {artist.Id} gefunden.");

            return await _artistRepository.UpdateAsync(artist);
        }

        public async Task DeleteAsync(string id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {id} gefunden.");

            await _artistRepository.DeleteAsync(id);
        }
    }
}
