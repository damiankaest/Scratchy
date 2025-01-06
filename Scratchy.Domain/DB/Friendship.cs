using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class Friendship
    {
        [BsonId]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("senderId")]
        public string SenderId { get; set; } // User, der die Anfrage gesendet hat
        [BsonElement("receiver")]
        public string ReceiverId { get; set; } // User, der die Anfrage erhalten hat
        [BsonElement("status")]
        public FriendshipStatus Status { get; set; } // Status der Freundschaft
        [BsonElement("createdAd")]
        public DateTime CreatedAt { get; set; } // Zeitstempel der Erstellung
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; } // Zeitstempel der letzten Aktualisierung
    }

    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected
    }

}
