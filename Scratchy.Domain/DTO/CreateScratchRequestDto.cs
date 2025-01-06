using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.DTO
{
    public class CreateScratchRequestDto
    {
        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
        [BsonElement("albumId")]
        public string AlbumId { get; set; } = string.Empty;
        [BsonElement("rating")]
        public float Rating { get; set; } = 0f;
        [BsonElement("userImageAsBase64String")]
        public string UserImageAsBase64String { get; set; } = string.Empty;
        
    }
}
