using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class WishlistItem
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Zu welcher Wunschliste gehört dieser Eintrag?
        /// </summary>
        public string WishlistId { get; set; }
        // public Wishlist Wishlist { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Album, das auf die Wunschliste gesetzt wurde (oder TrackId / ArtistId, je nach Bedarf).
        /// </summary>
        public string AlbumId { get; set; }
        // public Album Album { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Zeitpunkt, wann dieses Album zur Wishlist hinzugefügt wurde.
        /// </summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;
    }

}
