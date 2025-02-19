namespace Scratchy.Domain.DTO.Response
{
    public class CollectionAlbumScratchesDto
    {
        public int AlbumId { get; set; }
        public int ScratchId { get; set;}
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public float Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
