using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string? SpotifyId { get; set; }

        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }


        public DateTime? ReleaseDate { get; set; }

        [MaxLength(255)]
        public string CoverImageUrl { get; set; }

        [MaxLength(50)]
        public string? NfcTagId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual ICollection<Track> Tracks { get; set; }

        // Many-to-Many
        public virtual ICollection<Genre> Genres { get; set; }

        // Posts, Scratches, usw. die sich auf dieses Album beziehen könnten
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Scratch> Scratches { get; set; }
    }
}
