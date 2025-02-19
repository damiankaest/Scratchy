namespace Scratchy.Domain.DTO
{
    public class ScratchDetailsResponseDto
    {
        public int ScratchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; } = 0;
    }
}
