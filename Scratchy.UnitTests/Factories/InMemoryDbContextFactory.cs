using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Persistence.DB;
using System;

namespace Scratchy.UnitTests.Factories
{
    public static class InMemoryDbContextFactory
    {
        public static ScratchItDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ScratchItDbContext>()
                .UseInMemoryDatabase(databaseName: $"ScratchItDb_{Guid.NewGuid()}")
                .Options;

            return new ScratchItDbContext(options);
        }

        public static void SeedData(ScratchItDbContext context)
        {
            // Beispiel: Ein User und ein Album anlegen
            var user = new User
            {
                Username = "SeedUser",
                Email = "seeduser@example.com",
                CreatedAt = DateTime.UtcNow
            };

            var album = new Album
            {
                Title = "Seed Album",
                CoverImageUrl = "http://example.com/album.jpg",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            context.Albums.Add(album);
            context.SaveChanges();

            // Weitere Seed-Daten nach Bedarf hinzufügen
        }

    }
}
