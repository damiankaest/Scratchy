using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO.DB
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Falls du ein Symbol oder Bild hinterlegen willst:
        public string IconUrl { get; set; }

        // Navigation-Eigenschaft
        public virtual ICollection<UserBadge> UserBadges { get; set; }
    }
}
