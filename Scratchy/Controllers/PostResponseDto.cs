using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Scratchy.Domain.DTO;

using Scratchy.Domain.DTO.DB;

namespace Scratchy.Controllers
{
    public class PostResponseDto
    {
        public PostResponseDto(Post post, User _user)
        {
            _id = post.PostId;
            user = _user;
            CreatedOn = post.CreatedAt;
        }
        public int _id { get; set; } = 0;
        public User user { get; set; } = new User() { Username = "nAn"};
        public string mediaDataId { get; set; } = string.Empty;
        public string userRatingId { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string spotifyUrl { get; set; } = string.Empty;
        public string imgUrl { get; set; } = string.Empty;
        public int scratches { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}