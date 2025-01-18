using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO.DB
{
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Badge))]
        public int BadgeId { get; set; }
        public virtual Badge Badge { get; set; }

        // Weitere Felder, z.B. wann der User das Badge bekommen hat:
        public DateTime AwardedAt { get; set; }
    }
}
