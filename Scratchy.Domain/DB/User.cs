using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scratchy.Domain.DTO.Request;

namespace Scratchy.Domain.DB
{
    public class User
    {
        public User(CreateUserRequestDto createUserDto)
        {
            Username = createUserDto.UserName;
            Email = createUserDto.Email;
            Id = createUserDto.Uid;
        }

        public ICollection<UserBadge> UserBadges { get; set; }


        public User()
        {
            
        }

        [BsonId]
        public string Id { get; set; } = BsonObjectId.GenerateNewId().ToString();

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; } = string.Empty;

        [BsonElement("friends")]
        public List<string> Friends { get; set; } = new List<string>();

        [BsonElement("scratches")]
        public List<string> Scratches { get; set; } = new List<string>();
    }
}
