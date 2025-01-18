using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.DTO
{
    public class UserSearchResponseDTO
    {
        public UserSearchResponseDTO(ExploreUserDto user) {
            userName = user.UserName;
            ProfilePicture = user.UserImg;
            id = user.UserId;
        }
        [BsonElement("id")]
        public int id { get; set; }
        [BsonElement("username")]
        public string userName { get; set; }
        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; } = string.Empty;

    }
}
