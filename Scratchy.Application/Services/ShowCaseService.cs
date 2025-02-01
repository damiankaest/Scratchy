using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Application.Services
{
    public class ShowCaseService : IShowCaseService
    {
        private readonly IShowCaseRepository _showCaseRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IBadgeRepository _badgeRepository;

        public ShowCaseService(IShowCaseRepository showCaseRepository, IAlbumRepository albumRepository,IBadgeRepository badgeRepository,ITrackRepository _trackRepository)
        {
            _showCaseRepository = showCaseRepository;
            _albumRepository = albumRepository;
            _badgeRepository = badgeRepository;
            __trackRepository = _trackRepository;
        }

        public async Task<List<ShowCaseResponseDTO>> GetAllShowCasesFromUser(int userId)
        {
            var userShowCases = await _showCaseRepository.GetByUserId(userId);

            var showCaseResponse = new ShowCaseResponseDTO
            {
                AlbumShowCase = new List<AlbumShowCaseEntity>(),
                BadgeShowCase = new List<BadgeShowCaseEntity>(),
                TrackShowCase = new List<TrackShowCaseEntity>()
            };

            foreach (var showCase in userShowCases)
            {
                switch (showCase.Type)
                {
                    case ShowCaseType.Albums:
                        var albumIds = new List<int> { showCase.firstPlaceEntityId, showCase.secondPlaceEntityId, showCase.thirdPlaceEntityId };
                        var albums = await _albumRepository.GetByListOfIdAsync(albumIds);

                        showCaseResponse.AlbumShowCase = albums.Select(a => new AlbumShowCaseEntity
                        {
                            AlbumId = a.AlbumId,
                            AlbumName = a.Title,
                            Rating = 0,
                            ScratchCount = 0
                        }).ToList();
                        break;

                    case ShowCaseType.Badges:
                        var badgeIds = new List<int> { showCase.firstPlaceEntityId, showCase.secondPlaceEntityId, showCase.thirdPlaceEntityId };
                        var badges = await _badgeRepository.GetBadgesByIdsAsync(badgeIds);

                        showCaseResponse.BadgeShowCase = badges.Select(b => new BadgeShowCaseEntity
                        {
                            // Hier Badge spezifische Felder befüllen
                        }).ToList();
                        break;

                    case ShowCaseType.Tracks:
                        var trackIds = new List<int> { showCase.firstPlaceEntityId, showCase.secondPlaceEntityId, showCase.thirdPlaceEntityId };
                        var tracks = await _trackRepository.GetTracksByIdsAsync(trackIds);

                        showCaseResponse.TrackShowCase = tracks.Select(t => new TrackShowCaseEntity
                        {
                            // Hier Track spezifische Felder befüllen
                        }).ToList();
                        break;
                }
            }

            return showCaseResponse;
        }

        public Task<ShowCaseResponseDTO> GetByShowCaseIdAsync(int showCaseId)
        {
            throw new NotImplementedException();
        }

        Task<ShowCaseResponseDTO> IShowCaseService.GetAllShowCasesFromUser(int userId)
        {
            throw new NotImplementedException();
        }
    }

        public async Task<ShowCaseResponseDTO> GetByShowCaseIdAsync(int userId)
        {
            
    }
}
