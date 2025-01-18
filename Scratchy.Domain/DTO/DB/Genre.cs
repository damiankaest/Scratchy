using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DTO.DB
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        // Many-to-Many zu Albums
        public virtual ICollection<Album> Albums { get; set; }
    }
}
