namespace Scratchy.Controllers
{
    public class HomeFeedResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserImageUrl { get; set; } = string.Empty;
        public string PostImageUrl { get; set; } = string.Empty;
        public int Likes { get; set; }
        public string Caption { get; set; } = string.Empty;
        public int Comments { get; set; }
        public double Rating { get; set; } // Sterne-Bewertung
        public bool? IsLiked { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string AlbumImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string SpotifyUrl { get; set; } = string.Empty;
    }
}