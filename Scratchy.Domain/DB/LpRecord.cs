using Scratchy.Domain.Enum;

namespace Scratchy.Domain.DB
{
    public class LpRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string? Album { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public string? Label { get; set; } // Plattenlabel
        public string? CatalogNumber { get; set; } // Katalognummer
        public Condition RecordCondition { get; set; } // Zustand der Schallplatte
    }
}
