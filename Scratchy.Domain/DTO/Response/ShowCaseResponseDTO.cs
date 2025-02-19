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
        public int ShowCaseId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public int TrackId { get; set; }
    }

    public class BadgeShowCaseEntity
    {
        public int ShowCaseId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public int BadgeId { get; set; }
    }

    public class AlbumShowCaseEntity
    {
        public int ShowCaseId { get; set; } = 0;
        public int AlbumId { get; set; } = 0;
        public string AlbumName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
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
