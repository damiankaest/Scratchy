using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Scratch
    {

        [Key]
        public int ScratchId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }
        public int Rating { get; set; }
        public int LikeCounter { get; set; }
        public string? ScratchImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        // Many-to-Many zu Tags
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
