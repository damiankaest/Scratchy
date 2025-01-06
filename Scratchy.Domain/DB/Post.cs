using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.DTO;

namespace Scratchy.Domain.DB
{
    public class Post
    {
        public Post(CreateScratchRequestDto createPost)
        {
            _id = ObjectId.GenerateNewId().ToString();
            mediaDataId = createPost.AlbumId;
            userRatingId = ObjectId.GenerateNewId().ToString();
            title = "createPost.t";
            description = createPost.Description;
            userId = createPost.UserId;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string mediaDataId { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string userRatingId { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        public int scratches { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
