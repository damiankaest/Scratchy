namespace Scratchy.Domain.DTO.Response
{
    public class ShowCaseResponseDTO
    {
        public List<AlbumShowCaseEntity> AlbumShowCase { get; set; }
        public List<BadgeShowCaseEntity> BadgeShowCase { get; set; }
        public List<TrackShowCaseEntity> TrackShowCase { get; set; }
    }

    public class TrackShowCaseEntity
    {
    }

    public class BadgeShowCaseEntity
    {
    }

    public class AlbumShowCaseEntity
    {
        public int AlbumId { get; set; } = 0;
        public string AlbumName { get; set; } = "";
        public int Rating { get; set; } = 0;
        public int ScratchCount { get; set; } = 0;
    }

    public enum ShowCaseType
    {
        None,
        Badges,
        Albums,
        Tracks
    }

    public class ShowCaseResponseEntity
    {

        public int showCaseId { get; set; }
        public ShowCaseType showCaseType { get; set; }
        public int firstPlaceEntityId { get; set; }
        public int secondPlaceEntityId { get; set; }
        public int thirdPlaceEntityId { get; set; }
    }
}
