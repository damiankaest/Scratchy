using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DB;

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

        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Follow>()
                 .Property(f => f.Id)
                 .HasDefaultValueSql("NEWID()"); // SQL Server generiert GUID automatisch
        }
    }
}
