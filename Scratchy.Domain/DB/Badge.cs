using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class Badge
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserBadge> UserBadges { get; set; }

    }
}
