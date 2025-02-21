﻿namespace Scratchy.Domain.DTO.Response
{
    public class ArtistDetailsDto
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string ArtistImageUrl { get; set; }
        public List<PopularAlbumDto> PopularAlbums { get; set; }
        public List<RecentScratchDto> RecentScratches { get; set; }
    }

    public class PopularAlbumDto
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumImageUrl { get; set; }
        public int ReleaseYear { get; set; }
    }
}
