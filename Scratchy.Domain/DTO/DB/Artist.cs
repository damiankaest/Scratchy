using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DTO.DB
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        public string SpotifyId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string? Bio { get; set; }

        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual ICollection<Album> Albums { get; set; }
    }
}
