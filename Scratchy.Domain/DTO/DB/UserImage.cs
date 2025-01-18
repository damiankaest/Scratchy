using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class UserImage
    {
        [Key]
        public int UserImageId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
