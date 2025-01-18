using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public string Content { get; set; }

        // DECIMAL(2,1)
        [Column(TypeName = "decimal(2,1)")]
        public decimal? Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
