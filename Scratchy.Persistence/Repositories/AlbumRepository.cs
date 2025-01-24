using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.DTO.DB;
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

        public async Task<Album> GetByIdAsync(int id)
        {
            return await _context.Albums.FindAsync(id);
        }

        public async Task AddAsync(Album entity)
        {
            _context.Albums.Add(entity);
            await _context.SaveChangesAsync();
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
                var albumResult = await _context.Albums
    .Include(a => a.Artist) 
    .Where(a => EF.Functions.Like(a.Title, $"{query}%") ||
                EF.Functions.Like(a.Artist.Name, $"%{query}%")) 
    .Take(limit)
    .ToListAsync();

            if (!albumResult.Any())
            {
                
                    var albumList = await _spotifyService.SearchForAlbumByQuery(query);

                    foreach (var album in albumList)
                    {
                        var existingAlbum = await _context.Albums
                            
                            .FirstOrDefaultAsync(a => a.AlbumId == album.AlbumId && a.Title== album.Title);

                        if (existingAlbum == null)
                        {
                            _context.Albums.Add(album);
                            albumResult.Add(album);
                        }
                    }

                    await _context.SaveChangesAsync();
                }

            

            return albumResult.Take(limit).ToList();
                }
                            catch (Exception ex)
                {
                throw new ApplicationException("Fehler beim Abrufen von Alben von Spotify.", ex);
            }
        }

        public async Task<Album> GetBySpotifyIdAsync(string spotifyId)
        {
            return await _context.Albums
                .FirstOrDefaultAsync(a => a.SpotifyId == spotifyId);
        }
    }
}
