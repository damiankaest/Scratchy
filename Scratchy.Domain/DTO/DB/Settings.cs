using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Settings
    {
        [Key]
        public int SettingsId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsPublicProfile { get; set; }

        public bool ReceiveNotifications { get; set; }

        [MaxLength(10)]
        public string Language { get; set; }

        [MaxLength(20)]
        public string Theme { get; set; }
    }
}
