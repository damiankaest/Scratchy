namespace Scratchy.Controllers
{
    public class HomeFeedResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string PostImageUrl { get; set; }
        public int Likes { get; set; }
        public string Caption { get; set; }
        public int Comments { get; set; }
        public double Rating { get; set; } // Sterne-Bewertung
        public bool? IsLiked { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}