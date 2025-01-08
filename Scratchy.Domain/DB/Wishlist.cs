using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class Wishlist
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Der User, dem diese Wunschliste gehört.
        /// </summary>
        public string UserId { get; set; }
        // public User User { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Individueller Name / Titel der Wunschliste.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibt kurz, was in dieser Wunschliste gesammelt wird.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Wann die Wunschliste erstellt wurde.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Zeitpunkt, wann zuletzt etwas geändert wurde (optional).
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        // Optional: Navigation Property zu den Items
        // public ICollection<WishlistItem> WishlistItems { get; set; }
    }

}
