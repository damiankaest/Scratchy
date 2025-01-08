using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class PlaylistItem
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Zu welcher Playlist gehört dieser Eintrag?
        /// </summary>
        public string PlaylistId { get; set; }
        // public Playlist Playlist { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Referenz auf ein Album (oder Song), das in der Playlist enthalten ist.
        /// </summary>
        public string AlbumId { get; set; }
        // public Album Album { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Reihenfolge in der Playlist.
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Zeitpunkt, wann das Item zur Playlist hinzugefügt wurde.
        /// </summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;
    }

}
