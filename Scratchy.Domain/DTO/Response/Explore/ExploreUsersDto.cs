using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.DB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ExploreUserDto
    {

    public ExploreUserDto(User user)
    {
        UserId = user.Id;
        UserName = user.Username;
        UserImg = user.ProfilePicture;
        ScratchCount = 10;
        IsFollowing = false;
    }

    [BsonId]
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("userImg")]
        public string UserImg { get; set; }

        [BsonElement("scratchCount")]
        public int ScratchCount { get; set; }

        [BsonElement("isFollowing")]
        public bool IsFollowing { get; set; }
    }

