using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class Report
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Der Nutzer, der die Meldung abgegeben hat.
        /// </summary>
        public string ReporterUserId { get; set; }
        // public User Reporter { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Welche Art von Entity wurde gemeldet (z. B. "Comment", "Post", "User", "Review").
        /// </summary>
        public string ReportedEntityType { get; set; }

        /// <summary>
        /// ID der gemeldeten Entität (z. B. die ID eines Kommentars).
        /// </summary>
        public string ReportedEntityId { get; set; }

        /// <summary>
        /// Begründung oder kurze Beschreibung, warum gemeldet wurde.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Status der Meldung (z. B. "Open", "InReview", "Resolved").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Wann die Meldung erstellt wurde.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

}
