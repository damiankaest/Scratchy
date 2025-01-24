using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.DTO.Response
{
    public class ExploreAlbumDto
    {
        public string AlbumName { get; set; }
        public decimal AvgRating { get; set; }
        public int ScratchCount { get; set; }
        public int AlbumId { get; set; }
        public string ArtistName { get; set; }
        public string AlbumImageUrl { get; set; }

        public ExploreAlbumDto(Album album)
        {
            AlbumName = album.Title;
            AvgRating = 0;
            ScratchCount = 0;
            ArtistName = album.Artist.Name;
            AlbumId = album.AlbumId;
            AlbumImageUrl = album.CoverImageUrl;
        }
    }
}
