using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Persistence.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ScratchItDbContext _context;

        public ArtistRepository(ScratchItDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetByQueryAsync(string query, int limit)
        {
            try
            {
                var response = await _context.Artists
               .Where(a => a.Name.Contains(query)) // Filter basierend auf der Query
               .GroupBy(a => a.Name)              // Gruppieren nach ArtistName
               .Select(g => g.First())                  // Nur den ersten Eintrag jeder Gruppe auswählen
               .Take(limit)                             // Begrenzung der Ergebnisse
               .ToListAsync();

                return response;
            }
            catch (Exception EX)
            {

                throw;
            }
       
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist> GetByIdAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<Artist> AddAsync(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task<Artist> UpdateAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
