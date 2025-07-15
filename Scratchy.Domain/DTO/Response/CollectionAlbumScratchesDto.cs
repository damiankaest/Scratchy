namespace Scratchy.Domain.DTO.Response
{
    public class CollectionAlbumScratchesDto
    {
        public string AlbumId { get; set; }
        public string ScratchId { get; set;}
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public float Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
