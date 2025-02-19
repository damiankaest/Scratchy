using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class ShowCaseService : IShowCaseService
    {
        private readonly IShowCaseRepository _showCaseRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly ITrackRepository _trackRepository;

        public ShowCaseService(IShowCaseRepository showCaseRepository, IAlbumRepository albumRepository, IBadgeRepository badgeRepository, ITrackRepository trackRepository)
        {
            _showCaseRepository = showCaseRepository;
            _albumRepository = albumRepository;
            _badgeRepository = badgeRepository;
            _trackRepository = trackRepository;
        }

        public async Task<int> CreateShowCaseAsync(ShowCase createDto)
        {
            var result = await _showCaseRepository.AddAsync(createDto);
            return result.Id;
        }

        public async Task<ShowCaseResponseDTO> GetAllShowCasesFromUserByIdAsync(int userId)
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
                        showCaseResponse.AlbumShowCase = await GetAlbumInfoForAlbumShowCaseAsync(showCase);
                        break;

                    case ShowCaseType.Badges:
                        showCaseResponse.BadgeShowCase = await GetBadgesInfoForBadgesShowCaseAsync(showCase);
                        break;

                    case ShowCaseType.Tracks:
                        showCaseResponse.TrackShowCase = await GetTrackInfoForTracksShowCaseAsync(showCase);
                        break;
                }
            }

            return showCaseResponse;
        }

        private async Task<List<TrackShowCaseEntity>> GetTrackInfoForTracksShowCaseAsync(ShowCase showCase)
        {
            var tracksShowCase = new List<TrackShowCaseEntity>();

            var listofTrackIds = new List<int>() { showCase.FirstPlaceEntityId, showCase.SecondPlaceEntityId, showCase.ThirdPlaceEntityId };

            foreach (var id in listofTrackIds)
            {

                var trackInfo = await _trackRepository.GetByIdAsync(id);

                tracksShowCase.Add(new TrackShowCaseEntity()
                {
                    Title = trackInfo.Title,  
                    TrackId = id,
                });
            }

            return tracksShowCase;
        }


        private async Task<List<AlbumShowCaseEntity>> GetAlbumInfoForAlbumShowCaseAsync(ShowCase showCase)
        {
            var albumShowCase = new List<AlbumShowCaseEntity>();

            var listOfAlbumIds = new List<int>() { showCase.FirstPlaceEntityId, showCase.SecondPlaceEntityId, showCase.ThirdPlaceEntityId };

            foreach (var id in listOfAlbumIds)
            {

                var albumInfo = await _albumRepository.GetByIdAsync(id);

                albumShowCase.Add(new AlbumShowCaseEntity()
                {
                    ShowCaseId = showCase.Id,
                    AlbumId = albumInfo.AlbumId,
                    AlbumName = albumInfo.Title,
                    ImageUrl = albumInfo.CoverImageUrl
                });
            }

            return albumShowCase;
        }

        private async Task<List<BadgeShowCaseEntity>> GetBadgesInfoForBadgesShowCaseAsync(ShowCase showCase)
        {
            var badgesShowCase = new List<BadgeShowCaseEntity>();

            var listOfBadgesId = new List<int>() { showCase.FirstPlaceEntityId, showCase.SecondPlaceEntityId, showCase.ThirdPlaceEntityId };

            foreach (var id in listOfBadgesId)
            {

                var badgeInfo = await _badgeRepository.GetByIdAsync(id);

                badgesShowCase.Add(new BadgeShowCaseEntity()
                {
                    BadgeId = badgeInfo.Id,
                    Description = badgeInfo.Description,
                    Title = badgeInfo.Name
                });
            }

            return badgesShowCase;
        }

        public async Task<bool> CreateNewShowCaseAsync(CreateShowCaseRequestDto createDto, int userId)
        {
            await _showCaseRepository.AddAsync(new ShowCase()
            {
                CreatedAt = DateTime.UtcNow,
                FirstPlaceEntityId = createDto.FirstPlaceEntityId,
                SecondPlaceEntityId = createDto.SecondPlaceEntityId,
                ThirdPlaceEntityId = createDto.ThirdPlaceEntityId,
                UserId = userId,
                LastUpdate = DateTime.UtcNow,
                Type = createDto.ShowCaseType
            });
            return true;
        }

      public async Task<bool> UpdateShowcaseAsync(UpdateShowCaseDto updateDto)
        {
            var showCase = await _showCaseRepository.GetByIdAsync(updateDto.ShowCaseId);

            if (showCase == null)
            {
                return false;
            }

            showCase.Type = updateDto.ShowCaseType;
            showCase.FirstPlaceEntityId = updateDto.FirstPlaceEntityId;
            showCase.SecondPlaceEntityId = updateDto.SecondPlaceEntityId;
            showCase.ThirdPlaceEntityId = updateDto.ThirdPlaceEntityId;
            showCase.LastUpdate = DateTime.UtcNow; 

            await _showCaseRepository.UpdateAsync(showCase);

            return true;
        }
    }
}
