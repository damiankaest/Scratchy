using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Scratchy.Domain.DB;
using Scratchy.Domain.DTO;
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

        public async Task DeleteAsync(string id)
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

        public async Task<Scratch> GetByIdAsync(string id)
        {
            return await _context.Scratches.FindAsync(id);
        }

        public async Task<IEnumerable<Scratch>> GetByUserIdAsync(string userId)
        {
            return await _context.Scratches
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Scratch>> GetScratchesAsync(List<string> userIdList)
        {
            try
            {
                return await _context.Scratches
                    .Where(s => userIdList.Contains(s.UserId))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Scratch entity)
        {
            var existingScratch = await _context.Scratches.FindAsync(entity.Id);
            if (existingScratch != null)
            {
                // Update fields manually or with helper methods, depending on how you want to handle updates
                existingScratch.AlbumName = entity.AlbumName;
                existingScratch.ArtistName = entity.ArtistName;
                existingScratch.Rating = entity.Rating;
                existingScratch.LikeCount = entity.LikeCount;
                existingScratch.LikedBy = entity.LikedBy;
                existingScratch.Comments = entity.Comments;
                existingScratch.UserImageUrl = entity.UserImageUrl;
                existingScratch.AlbumImageUrl = entity.AlbumImageUrl;
                existingScratch.SpotifyRefUrl = entity.SpotifyRefUrl;
                existingScratch.CreatedOn = entity.CreatedOn;

                await _context.SaveChangesAsync(); // Save changes to the database
            }
        }

        public async Task<CreateScratchResponseDto> UploadAsync(CreateScratchRequestDto newScratch)
        {
            var scratch = new Scratch(newScratch);
            var response  = new CreateScratchResponseDto(true, "success");
            await _context.AddAsync(scratch);
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
