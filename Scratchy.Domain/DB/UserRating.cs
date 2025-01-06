using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.DB
{
    public class UserRating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string mediaDataId { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; } = string.Empty;
        public string rating { get; set; } = string.Empty;
        public DateTime createdOn { get; set; } = DateTime.Now;
    }
}
