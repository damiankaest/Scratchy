using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DB
{
    public class UserDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; } = string.Empty;
        public string details { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string img { get; set; } = string.Empty;

    }
}
