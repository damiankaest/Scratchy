using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.DB
{
    public class Comment
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("user_id")]
        public string UserId { get; set; } = string.Empty;
        [BsonElement("comment")]
        public string CommentText { get; set; } = string.Empty;
        [BsonElement("date")]
        public DateTime Date { get; set; }
    }

}
