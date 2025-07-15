using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.DTO.Response
{
    public class ExploreAlbumDto
    {
        public string AlbumName { get; set; }
        public decimal AvgRating { get; set; }
        public int ScratchCount { get; set; }
        public string AlbumId { get; set; }
        public string ArtistName { get; set; }
        public string AlbumImageUrl { get; set; }

        public ExploreAlbumDto(AlbumDocument album)
        {
            AlbumName = album.Title;
            AvgRating = 0;
            ScratchCount = 0;
            ArtistName = album.Artist.Name;
            AlbumId = album.Id;
            AlbumImageUrl = album.CoverImageUrl;
        }
    }
}
