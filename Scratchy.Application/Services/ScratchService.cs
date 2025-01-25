using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Services;
using System.Security.Claims;

namespace Scratchy.Application.Services
{
    public class ScratchService : IScratchService
    {
        private readonly IScratchRepository _scratchRepository;
        private readonly IUserService _userService;
        private readonly IAlbumService _albumService;

        public ScratchService(IScratchRepository scratchRepository, IUserService userService, IAlbumService albumService)
        {
            _scratchRepository = scratchRepository;
            _userService = userService;
            _albumService = albumService;
        }
        public async Task<bool> CreateNewAsync(CreateScratchRequestDto newScratch, User currentUser)
        {


            // 2. Album-Objekt über den albumService abrufen (optional, falls AlbumId angegeben ist)
            Album? album = null;
            if (newScratch.AlbumId!= null)
            {


                try
                {
                    album = await _albumService.GetByIdAsync(newScratch.AlbumId);
                }
                catch (Exception ex)
                {

                    throw;
                }
                
                if (album == null)
                {
                    throw new ArgumentException($"Album mit ID {newScratch.AlbumId} wurde nicht gefunden.");
                }
            }

            // 3. Neues Scratch-Objekt erstellen
            var scratch = new Scratch
            {
                UserId = currentUser.UserId,
                User = currentUser,
                AlbumId = album?.AlbumId,
                Album = album,
                Title = (album?.Title ?? "No Album"), // Beispiel-Title
                Content = newScratch.Description,
                Rating = (int)Math.Round(newScratch.Rating),
                LikeCounter = 0,
                CreatedAt = DateTime.UtcNow,
                Tags = new List<Tag>() // Initialisiere Tags, falls später hinzugefügt werden
            };

            // 4. Scratch speichern
            await _scratchRepository.AddAsync(scratch);

            // 5. Erfolg zurückgeben
            return true;
        }

        public Task<bool> DeleteScratchByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Scratch>> GetByUserIdAsync(int userId)
        {
            return _scratchRepository.GetByUserIdAsync(userId);
        }

        public Task<ScratchDetailsResponseDto> GetDetailsById(string scratchId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Scratch>> GetHomeFeedByUserIdListAsync(List<int> homeFeedUserIdList)
        {
            var homeFeed = await _scratchRepository.GetScratchesAsync(homeFeedUserIdList);
            return homeFeed;
        }
    }
}
