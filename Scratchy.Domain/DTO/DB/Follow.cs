using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class Follow
    {
        // Composite Key (FollowerId, FollowedId) -> per Fluent API in OnModelCreating
        [ForeignKey(nameof(Follower))]
        public int FollowerId { get; set; }
        public virtual User Follower { get; set; }

        [ForeignKey(nameof(Followed))]
        public int FollowedId { get; set; }
        public virtual User Followed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
