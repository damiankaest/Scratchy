﻿using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

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
        public async Task<Scratch> CreateNewAsync(CreateScratchRequestDto newScratch, User currentUser)
        {
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
            var result = await _scratchRepository.AddAsync(scratch);

            // 5. Erfolg zurückgeben
            return result;
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

        public async Task<bool> SetUserImageURLOnScratch(Scratch entity, string imgUrl)
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

        //Task<Scratch> IScratchService.CreateNewAsync(CreateScratchRequestDto newScratch, User currUser)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
