using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.DTO.Request;

namespace Scratchy.Domain.DB
{
    public class Notification
    {
        public Notification()
        {
                
        }
        public Notification(NewMessageDto message)
        {
            Id = ObjectId.GenerateNewId().ToString();
            ReceiverId = message.ReceiverId;
            SenderId = message.SenderId;
            Type = message.Type;
            Message = message.Message;
            HasSeen = false;
            CreatedAt = DateTime.Now;
        }

        [BsonId]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;
        [BsonElement("receiverId")]
        public string ReceiverId { get; set; } = string.Empty;
        [BsonElement("senderId")]
        public string SenderId { get; set; } = string.Empty;
        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;
        [BsonElement("message")]
        public string Message{ get; set; } = string.Empty;
        [BsonElement("hasSeen")]
        public bool HasSeen { get; set; } = false;
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
