using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DB;

namespace Scratchy.Controllers
{
    public class PostResponseDto
    {
        public PostResponseDto(Post post, User _user)
        {
            _id = post._id;
            mediaDataId = post.mediaDataId;
            userRatingId = post.userRatingId;
            title = post.title;
            description = post.description;
            user = _user;
            CreatedOn = post.CreatedOn;
            scratches = post.scratches;
        }
        public string _id { get; set; } = string.Empty;
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