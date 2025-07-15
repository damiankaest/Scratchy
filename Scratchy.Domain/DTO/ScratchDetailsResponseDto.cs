namespace Scratchy.Domain.DTO
{
    public class ScratchDetailsResponseDto
    {
        public string ScratchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public string AlbumId { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; } = 0;
    }
}
