using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class PlaylistTracks
    {
        [ForeignKey(nameof(Playlist))]
        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        [ForeignKey(nameof(Track))]
        public int TrackId { get; set; }
        public virtual Track Track { get; set; }

        public int Order { get; set; }
    }
}
