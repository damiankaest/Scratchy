using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DB;
using System.Reflection.Emit;

namespace Scratchy.Persistence.DB
{
    public class ScratchItDbContext : DbContext
    {
        public ScratchItDbContext(DbContextOptions<ScratchItDbContext> options)
            : base(options)
        {
        }

        // Ihre DbSets
        public DbSet<Scratch> Scratches { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Follow> Follows { get; set; }

        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Badge> Badges { get; set; }

        public DbSet<Library> Libraries { get; set; }

        // upcoming Features:
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<MarketplaceListing> MarketplaceListings { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistItem> PlaylistItems { get; set; }
        public DbSet<Report> Reports { get; set; }






        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Follow>()
                 .Property(f => f.Id)
                 .HasDefaultValueSql("NEWID()"); // SQL Server generiert GUID automatisch

            builder.Entity<Badge>(entity =>
            {
                entity.HasKey(b => b.Id);
            });

            builder.Entity<UserBadge>(entity =>
            {
                entity.HasKey(ub => ub.Id);

                // Relationen
                entity
                    .HasOne(ub => ub.User)
                    .WithMany(u => u.UserBadges)
                    .HasForeignKey(ub => ub.UserId);

                entity
                    .HasOne(ub => ub.Badge)
                    .WithMany(b => b.UserBadges)
                    .HasForeignKey(ub => ub.BadgeId);
            });
        }
    }
}
