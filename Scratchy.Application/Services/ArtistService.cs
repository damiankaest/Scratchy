using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

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

            foreach (var artist in await _artistRepository.GetByQueryAsync(query, limit))
            {
                artistExplorer.Add(new ExploreArtistsDto(artist));
            }

            return artistExplorer;
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _artistRepository.GetAllAsync();
        }

        public async Task<Artist> GetByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {id} gefunden.");

            return artist;
        }

        public async Task<Artist> AddAsync(Artist artist)
        {
            if (artist == null)
                throw new ArgumentNullException(nameof(artist));

            return await _artistRepository.AddAsync(artist);
        }

        public async Task<Artist> UpdateAsync(Artist artist)
        {
            if (artist == null)
                throw new ArgumentNullException(nameof(artist));

            var existingArtist = await _artistRepository.GetByIdAsync(1);
            if (existingArtist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {artist.ArtistId} gefunden.");

            return await _artistRepository.UpdateAsync(artist);
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
                throw new KeyNotFoundException($"Kein Künstler mit der ID {id} gefunden.");

            await _artistRepository.DeleteAsync(id);
        }
    }
}
