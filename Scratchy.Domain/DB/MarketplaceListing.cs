using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class MarketplaceListing
    {
        /// <summary>
        /// Primärschlüssel (string).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Wer bietet diese Schallplatte an?
        /// </summary>
        public string SellerUserId { get; set; }
        // public User Seller { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Bezieht sich auf ein bestimmtes Album (oder Schallplatte).
        /// </summary>
        public string AlbumId { get; set; }
        // public Album Album { get; set; } // Optional: Navigation Property

        /// <summary>
        /// Preis, falls das Album verkauft wird.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Beschreibung zum Zustand des Albums.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Flag: Angebot zum Verkauf.
        /// </summary>
        public bool IsForSale { get; set; }

        /// <summary>
        /// Flag: Angebot zum Tauschen.
        /// </summary>
        public bool IsForTrade { get; set; }

        /// <summary>
        /// Zeitstempel bei Erstellung des Angebots.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Letzte Änderung (z. B. wenn der Anbieter den Preis anpasst).
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

}
