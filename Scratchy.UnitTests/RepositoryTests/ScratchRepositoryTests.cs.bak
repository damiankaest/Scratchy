using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Persistence.DB;
using Scratchy.Persistence.Repositories;
using Scratchy.UnitTests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.UnitTests.RepositoryTests
{
    [TestFixture]
    public class ScratchRepositoryTests
    {
        private ScratchItDbContext _dbContext;
        private ScratchRepository _scratchRepository;



        [SetUp]
        public void SetUp()
        {
            _dbContext = InMemoryDbContextFactory.Create();
            
            _scratchRepository = new ScratchRepository(_dbContext);
            InMemoryDbContextFactory.SeedData(_dbContext);
        }


        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddScratch_ShouldAddScratchToDatabase()
        {
            var user = _dbContext.Users.First(); // Vom SeedData
            var album = _dbContext.Albums.First(); // Vom SeedData
            var scratch = new Scratch
            {
                
                UserId = user.UserId,
                AlbumId = album.AlbumId,
                Title = "New Scratch",
                Content = "This is a new scratch content.",
                Rating = 5,
                LikeCounter = 10,
                CreatedAt = DateTime.UtcNow,
                Tags = new List<Tag>
                {
                    new Tag { Name = "CSharp" },
                    new Tag { Name = "UnitTest" }
                }
            };

            await _scratchRepository.AddAsync(scratch);
            var scratchInDb = await _dbContext.Scratches
                   .Include(s => s.Tags)
                   .FirstOrDefaultAsync(s => s.ScratchId == scratch.ScratchId);

            Assert.IsNotNull(scratchInDb);
            Assert.AreEqual("New Scratch", scratchInDb.Title);
            Assert.AreEqual("This is a new scratch content.", scratchInDb.Content);
            Assert.AreEqual(5, scratchInDb.Rating);
            Assert.AreEqual(10, scratchInDb.LikeCounter);
            Assert.IsNotNull(scratchInDb.Tags);
            Assert.AreEqual(2, scratchInDb.Tags.Count);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnScratch()
        {
            // Arrange
            var user = _dbContext.Users.First();
            var scratch = new Scratch
            {
                UserId = user.UserId,
                Title = "Existing Scratch",
                Content = "Existing content.",
                Rating = 4,
                LikeCounter = 5,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Scratches.AddAsync(scratch);
            await _dbContext.SaveChangesAsync();

            // Act
            var retrievedScratch = await _scratchRepository.GetByIdAsync(scratch.ScratchId);

            // Assert
            Assert.IsNotNull(retrievedScratch);
            Assert.AreEqual("Existing Scratch", retrievedScratch.Title);
        }

    }
}
