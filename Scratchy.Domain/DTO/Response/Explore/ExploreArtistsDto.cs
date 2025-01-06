namespace Scratchy.Domain.DTO.Response.Explore
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ExploreArtistsDto
    {
        public ExploreArtistsDto(Artist artist)
        {
            ArtistId = artist.Id;
            ArtistName = artist.Name;
            ArtistImg = artist.ImageUrl;
            AlbumCount = 0;
            ScratchCount = 0;
            
        }
        [BsonId]
        [BsonElement("artistId")]
        public string ArtistId { get; set; }

        [BsonElement("artistName")]
        public string ArtistName { get; set; }

        [BsonElement("artistImg")]
        public string ArtistImg { get; set; }

        [BsonElement("albumCount")]
        public int AlbumCount { get; set; }

        [BsonElement("scratchCount")]
        public int ScratchCount { get; set; }

        [BsonElement("isFollowing")]
        public bool IsFollowing { get; set; }
    }

}
