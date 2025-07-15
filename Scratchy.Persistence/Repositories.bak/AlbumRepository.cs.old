using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Exceptions;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ScratchItDbContext _context;
        private readonly ISpotifyService _spotifyService;

        public AlbumRepository(ScratchItDbContext context, ISpotifyService spotifyService)
        {
            _context = context;
            _spotifyService = spotifyService;
        }

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            return await _context.Albums.ToListAsync();
        }

        public async Task<AlbumDetailsDto> GetDetailsByIdAsync(int albumId)
        {
            var album = await _context.Albums
                .Where(a => a.AlbumId == albumId)
                .Include(a => a.Artist)
                .Include(a => a.Tracks)
                .Include(a => a.Scratches)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync();

            if (album == null)
                return null;

            return new AlbumDetailsDto
            {
                AlbumId = album.AlbumId,
                AlbumName = album.Title,
                AlbumImageUrl = album.CoverImageUrl,
                ArtistName = album.Artist?.Name ?? "Unknown",
                ReleaseYear = album.ReleaseDate?.Year ?? 0,
                AverageRating = album.Scratches.Any() ? album.Scratches.Average(s => s.Rating) : 0,
                ScratchCount = album.Scratches.Count,
                SpotifyUrl = "https://open.spotify.com/intl-de/album/"+album.SpotifyId,
                Tracks = album.Tracks.Select(t => new TrackDto
                {
                    TrackId = t.TrackId,
                    TrackName = t.Title,
                    Duration = "2:00",
                    TrackNumber = t.TrackNumber
                }).ToList(),
                RecentScratches = album.Scratches
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(5)
                    .Select(s => new RecentScratchDto
                    {
                        ScratchId = s.ScratchId,
                        UserName = s.User?.Username ?? "Unknown",
                        UserImageUrl = s.User?.ProfilePictureUrl?? string.Empty,
                        Rating = s.Rating,
                        Description = s.Content,
                        CreatedAt = s.CreatedAt
                    }).ToList()
            };
        }

        public async Task<Album> AddAsync(Album entity)
        {
            _context.Albums.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Album entity)
        {
            _context.Albums.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var album = await GetByIdAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Album>> GetByQueryAsync(string query, int limit = 15)
        {
            try
            {
                // 1. Suche Alben aus der Datenbank
                var dbAlbums = await _context.Albums
                    .Include(a => a.Artist)
                    .Where(a => EF.Functions.Like(a.Title, $"{query}%") ||
                                EF.Functions.Like(a.Artist.Name, $"%{query}%"))
                    .Take(limit)
                    .ToListAsync();

                // 2. Falls noch nicht genug Alben vorhanden sind, ergänze mit Spotify-Daten
                int missingCount = limit - dbAlbums.Count;
                if (missingCount > 0)
                {
                    var spotifyAlbums = await _spotifyService.SearchForAlbumByQuery(query, missingCount);

                    // 3. Entferne doppelte Alben (die bereits in der DB existieren)
                    var uniqueSpotifyAlbums = spotifyAlbums
                        .Where(sa => !dbAlbums.Any(dbA => dbA.SpotifyId == sa.SpotifyId))
                        .ToList();

                    // 4. Falls Alben noch nicht in der DB sind, speichere sie
                    foreach (var album in uniqueSpotifyAlbums)
                    {
                        var existingAlbum = await _context.Albums
                            .Include(a => a.Artist)
                            .FirstOrDefaultAsync(a => a.SpotifyId == album.SpotifyId);

                        if (existingAlbum == null)
                        {
                            var existingArtist = await _context.Artists
                                .FirstOrDefaultAsync(a => a.SpotifyId == album.Artist.SpotifyId);

                            if (existingArtist != null)
                            {
                                album.Artist = existingArtist; // Nutze existierenden Artist
                            }

                            _context.Albums.Add(album);
                            dbAlbums.Add(album); // Füge es zur Rückgabeliste hinzu
                        }
                    }

                    await _context.SaveChangesAsync(); // Speichere neue Alben in die DB
                }

                return dbAlbums.Take(limit).ToList();
            }
            catch (Exception ex)
            {
                throw new SearchServiceException($"Fehler beim Abrufen von Alben: {ex.Message}");
            }
        }
        public async Task<Album> GetBySpotifyIdAsync(string spotifyId)
        {
            return await _context.Albums
                .FirstOrDefaultAsync(a => a.SpotifyId == spotifyId);
        }

        public async Task<Album> GetByIdAsync(int albumId)
        {
            return await _context.Albums
                           .FirstOrDefaultAsync(a => a.AlbumId == albumId);
        }
    }
}
