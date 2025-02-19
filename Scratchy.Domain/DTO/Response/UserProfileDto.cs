using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO.Response
{
    public class UserProfileDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("userImageUrl")]
        public string UserImageUrl { get; set; }

        [JsonPropertyName("scratchCount")]
        public int ScratchCount { get; set; }

        [JsonPropertyName("followersCount")]
        public int FollowersCount { get; set; }

        [JsonPropertyName("followingCount")]
        public int FollowingCount { get; set; }

        [JsonPropertyName("recentScratches")]
        public List<RecentScratches> RecentScratches { get; set; }

        [JsonPropertyName("isFollowing")]
        public bool IsFollowing { get; set; }
    }

    public class RecentScratches
    {
        [JsonPropertyName("scratchId")]
        public int ScratchId { get; set; }

        [JsonPropertyName("albumName")]
        public string AlbumName { get; set; }

        [JsonPropertyName("albumImageUrl")]
        public string AlbumImageUrl { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        // Je nach Bedarf kannst du hier DateTime verwenden, sofern das Datum konvertiert werden kann.
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
