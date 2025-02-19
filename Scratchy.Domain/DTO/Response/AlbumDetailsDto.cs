using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO.Response
{
    public class AlbumDetailsDto
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string AlbumImageUrl { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public double AverageRating { get; set; }
        public int ScratchCount { get; set; }
        public string SpotifyUrl { get; set; } = string.Empty;
        public List<TrackDto> Tracks { get; set; } = new();
        public List<RecentScratchDto> RecentScratches { get; set; } = new();
    }

    public class TrackDto
    {
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int TrackNumber { get; set; }
    }

    public class RecentScratchDto
    {
        public int ScratchId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}
