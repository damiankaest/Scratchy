using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class Playlist
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Zu welchem Nutzer gehört diese Playlist?
        /// </summary>
        public string UserId { get; set; }
        // public User User { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Name / Titel der Playlist (z. B. "Meine Lieblings-Rock-Platten").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Steuert, ob die Playlist öffentlich sichtbar ist.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Erstellt-Datum.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Wann zuletzt etwas geändert wurde (z. B. Items).
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        // Optional: Navigation Property zu den Items
        // public ICollection<PlaylistItem> PlaylistItems { get; set; }
    }

}
