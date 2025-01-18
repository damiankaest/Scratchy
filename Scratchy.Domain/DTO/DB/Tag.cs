using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DTO.DB
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        // Many-to-Many zu Scratch
        public virtual ICollection<Scratch> Scratches { get; set; }
    }
}
