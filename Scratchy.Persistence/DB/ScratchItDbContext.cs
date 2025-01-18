using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using System.Reflection.Emit;

namespace Scratchy.Persistence.DB
{
    public class ScratchItDbContext : DbContext
    {
        public ScratchItDbContext(DbContextOptions<ScratchItDbContext> options)
            : base(options)
        {
        }

        // -- DbSets für deine Entities --
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Scratch> Scratches { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ScratchesTags> ScratchesTags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AlbumGenres> AlbumGenres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTracks> PlaylistTracks { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- Follow: Composite-Key (FollowerId, FollowedId)
            builder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FollowedId });

            // --- ScratchesTags: Composite-Key (ScratchId, TagId)
            builder.Entity<ScratchesTags>()
                .HasKey(st => new { st.ScratchId, st.TagId });

            // --- AlbumGenres: Composite-Key (AlbumId, GenreId)
            builder.Entity<AlbumGenres>()
                .HasKey(ag => new { ag.AlbumId, ag.GenreId });

            // --- PlaylistTracks: Composite-Key (PlaylistId, TrackId)
            builder.Entity<PlaylistTracks>()
                .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            // Beispiel für Badge & UserBadge:
            // falls Badge.Id dein Primary Key ist
            builder.Entity<Badge>(entity =>
            {
                entity.HasKey(b => b.Id);
            });

            // Falls UserBadge.Id (PK) existiert, oder alternativ (UserId, BadgeId) als Composite Key
            builder.Entity<UserBadge>(entity =>
            {
                entity.HasKey(ub => ub.Id);

                // Relation: Ein UserBadge gehört zu genau einem User
                entity
                    .HasOne(ub => ub.User)
                    .WithMany(u => u.UserBadges)
                    .HasForeignKey(ub => ub.UserId);

                // Relation: Ein UserBadge gehört zu genau einem Badge
                entity
                    .HasOne(ub => ub.Badge)
                    .WithMany(b => b.UserBadges)
                    .HasForeignKey(ub => ub.BadgeId);
            });

            // Beispiel-Relation für User -> Follow:
            builder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithOne(f => f.Followed)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                .HasMany(u => u.Followings)
                .WithOne(f => f.Follower)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Comment>()
       .HasOne(c => c.Post)
       .WithMany(p => p.Comments)
       .HasForeignKey(c => c.PostId)
       .OnDelete(DeleteBehavior.Cascade);  // ggf. Kaskade, wenn du möchtest

            // Comments -> Users
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // KEINE Kaskade hier

            builder.Entity<Comment>()
    .HasOne(c => c.User)
    .WithMany(u => u.Comments)
    .HasForeignKey(c => c.UserId)
    .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Notification>()
    .HasOne(n => n.User)
    .WithMany(u => u.ReceivedNotifications)
    .HasForeignKey(n => n.UserId)
    .OnDelete(DeleteBehavior.ClientSetNull);

            // Relationship: Notification -> User (als Sender)
            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}