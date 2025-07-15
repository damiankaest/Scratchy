using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

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
        public async Task<ScratchDocument> CreateNewAsync(CreateScratchRequestDto newScratch, UserDocument currentUser)
        {
            AlbumDocument? album = null;
            if (newScratch.AlbumId != "0")
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
            var scratch = new ScratchDocument
            {
                User =new UserReference(){
                     UserId = currentUser.Id,

                },
                Album = new AlbumReference()
                {
                    AlbumId = album.Id,
                },
                Title = (album?.Title ?? "No Album"), // Beispiel-Title
                Content = newScratch.Description,
                Rating = (int)Math.Round(newScratch.Rating),
                LikeCounter = 0,
                CreatedAt = DateTime.UtcNow,
                Tags = new List<string>() // Initialisiere Tags, falls später hinzugefügt werden
            };

            try
            {
                // 4. Scratch speichern
                var result = await _scratchRepository.CreateAsync(scratch);
                return result;
            }
            catch
            {
                throw;
            }


        }

        public Task<bool> DeleteScratchByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CollectionAlbumScratchesDto>> GetAlbumScratchesForUserAsync(string albumId, string userId)
        {
            var scratches = await _scratchRepository.GetByUserAndAlbumIdIdAsync(userId, albumId);

            var albumScratches = scratches
                .Select(s => new CollectionAlbumScratchesDto
                {
                    CreatedAt = s.CreatedAt,
                    AlbumId = albumId,
                    ScratchId = s.Id,
                    ImageUrl = s.ScratchImageUrl,
                    Rating = s.Rating,
                    UserId = userId
                })
                .ToList();

            return albumScratches;
        }

        public Task<IEnumerable<ScratchDocument>> GetByUserIdAsync(string userId)
        {
            return _scratchRepository.GetByUserIdAsync(userId);
        }

        public async Task<ScratchDetailsResponseDto> GetDetailsById(string scratchId)
        {
            var scratch = await _scratchRepository.GetByIdAsync(scratchId.ToString());
            var result = new ScratchDetailsResponseDto()
            {
                ScratchId = scratch.Id,
                AlbumId = scratch.Album.AlbumId,
                CreatedAt = scratch.CreatedAt,
                ImageUrl = scratch.ScratchImageUrl,
                Rating = scratch.Rating,
                UserId = scratch.User.UserId,
                Title = scratch.Title
            };
            return result;
        }

        public async Task<IEnumerable<ScratchDocument>> GetHomeFeedByUserIdListAsync(List<string> homeFeedUserIdList)
        {
            var homeFeed = await _scratchRepository.GetScratchesAsync(homeFeedUserIdList);
            return homeFeed;
        }

        public async Task<List<AlbumShowCaseEntity>> GetIndividualAlbumsByUserIdAsync(string userId)
        {
            var scratches = await _scratchRepository.GetByUserIdAsync(userId);

            var albums = scratches
                .Select(s => s.Album) // Erst alle Alben extrahieren
                .GroupBy(a => a.AlbumId)   // Nach AlbumId gruppieren
                .Select(g => g.First()) // Pro Album nur ein Element behalten
                .Select(a => new AlbumShowCaseEntity
                {
                    AlbumId = a.AlbumId,
                    AlbumName = a.Title,
                    ImageUrl = a.CoverImageUrl
                })
                .ToList();

            return albums;
        }

        public async Task<bool> SetUserImageURLOnScratch(ScratchDocument entity, string imgUrl)
        {
            entity.ScratchImageUrl = imgUrl;
            try
            {
                await _scratchRepository.UpdateAsync(entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        Task<IEnumerable<ScratchDocument>> IScratchService.GetHomeFeedByUserIdListAsync(List<string> homeFeedUserIds)
        {
            throw new NotImplementedException();
        }

        //Task<Scratch> IScratchService.CreateNewAsync(CreateScratchRequestDto newScratch, User currUser)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
