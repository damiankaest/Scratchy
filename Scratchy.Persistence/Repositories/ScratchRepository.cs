using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class ScratchRepository : IScratchRepository
    {
        private readonly ScratchItDbContext _context;

        public ScratchRepository(ScratchItDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Scratch entity)
        {
            await _context.Scratches.AddAsync(entity);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task DeleteAsync(int id)
        {
            var scratch = await _context.Scratches.FindAsync(id);
            if (scratch != null)
            {
                _context.Scratches.Remove(scratch);
                await _context.SaveChangesAsync(); // Save changes to the database
            }
        }

        public async Task<IEnumerable<Scratch>> GetAllAsync()
        {
            return await _context.Scratches.ToListAsync();
        }

        public async Task<Scratch> GetByIdAsync(int id)
        {
            return await _context.Scratches.FindAsync(id);
        }


        public async Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId)
        {
            return await _context.Scratches
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Scratch>> GetScratchesAsync(List<int> userIdList)
        {
            try
            {
                return await _context.Scratches
                    .Include(s => s.Album )
                        .ThenInclude(a => a.Artist) // Einbindung von Artist
                    .Include(s => s.User) // Einbindung von User
                    .Where(s => userIdList.Contains(s.User.UserId))
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<Scratch>> GetScratchesAsync(List<string> userIdList)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Scratch entity)
        {
            var existingScratch = await _context.Scratches.FindAsync(entity.ScratchId);
            if (existingScratch != null)
            {
                // Update fields manually or with helper methods, depending on how you want to handle updates
                existingScratch.Album.Title = entity.Album.Title;
                existingScratch.Album.Title = entity.Album.Artist.Name;
                existingScratch.Rating = entity.Rating;
                existingScratch.Album.Title = entity.Album.Title;
                existingScratch.LikeCounter = entity.LikeCounter;
                existingScratch.Album.Title = entity.Album.Title;
                await _context.SaveChangesAsync(); // Save changes to the database
            }
        }

        public async Task<CreateScratchResponseDto> UploadAsync(CreateScratchRequestDto newScratch)
        {
            //var scratch = new Scratch(newScratch);
            var response  = new CreateScratchResponseDto(true, "success");
            //await _context.AddAsync(scratch);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return response;
        }
    }
}
