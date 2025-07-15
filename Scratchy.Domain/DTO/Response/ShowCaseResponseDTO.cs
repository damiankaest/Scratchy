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
        public string ShowCaseId { get; set; } = "0";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string TrackId { get; set; }
    }

    public class BadgeShowCaseEntity
    {
        public string ShowCaseId { get; set; } = "0";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string BadgeId { get; set; }
    }

    public class AlbumShowCaseEntity
    {
        public string ShowCaseId { get; set; } = "0";
        public string AlbumId { get; set; } = "0";
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

        public string showCaseId { get; set; }
        public ShowCaseType showCaseType { get; set; }
        public string firstPlaceEntityId { get; set; }
        public string secondPlaceEntityId { get; set; }
        public string thirdPlaceEntityId { get; set; }
    }
}
