using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.DTO.DB
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string? FirebaseId { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(256)]
        public string? PasswordHash { get; set; }

        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Follow> Followers { get; set; }       // User wird gefolgt
        public virtual ICollection<Follow> Followings { get; set; }      // User folgt anderen
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UserImage> UserImages { get; set; }
        public virtual ICollection<Notification> ReceivedNotifications { get; set; }
        public virtual ICollection<Notification> SentNotifications { get; set; }
        public virtual Settings Settings { get; set; }
        public virtual ICollection<Scratch> Scratches { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<UserBadge> UserBadges { get; set; }
    }
}
