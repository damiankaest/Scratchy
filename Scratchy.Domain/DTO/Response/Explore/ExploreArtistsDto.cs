namespace Scratchy.Domain.DTO.Response.Explore
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Scratchy.Domain.DTO.DB;

    public class ExploreArtistsDto
    {
        public ExploreArtistsDto(Artist artist)
        {
            ArtistId = artist.ArtistId;
            ArtistName = artist.Name;
            AlbumCount = 0;
            ScratchCount = 0;
            ArtistImg = artist.ProfilePictureUrl;
        }
        [BsonId]
        [BsonElement("artistId")]
        public int ArtistId { get; set; }

        [BsonElement("artistName")]
        public string ArtistName { get; set; }

        [BsonElement("artistImg")]
        public string ArtistImg { get; set; } = string.Empty;

        [BsonElement("albumCount")]
        public int AlbumCount { get; set; }

        [BsonElement("scratchCount")]
        public int ScratchCount { get; set; }

        [BsonElement("isFollowing")]
        public bool IsFollowing { get; set; }
    }

}
