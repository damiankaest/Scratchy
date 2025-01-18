using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        [ForeignKey(nameof(Album))]
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        // TIME(…) in SQL -> in .NET kann das ein TimeSpan sein
        public TimeSpan? Duration { get; set; }

        public int TrackNumber { get; set; }

        // Navigation zu Playlists (Many-to-Many)
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
