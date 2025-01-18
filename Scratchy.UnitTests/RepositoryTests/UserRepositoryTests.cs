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
    public class UserRepositoryTests
    {
        private ScratchItDbContext _dbContext;
        private UserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContext = InMemoryDbContextFactory.Create();
            _userRepository = new UserRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var newUser = new User
            {
                Username = "TestUser",
                Email = "testuser@example.com",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _userRepository.AddAsync(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.UserId, "UserId should be set after insertion.");

            var userInDb = await _dbContext.Users.FindAsync(result.UserId);
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("TestUser", userInDb.Username);
            Assert.AreEqual("testuser@example.com", userInDb.Email);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingUser()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "OldName",
                Email = "old@example.com",
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            // Act
            existingUser.Username = "NewName";
            existingUser.Email = "new@example.com";

            var updatedUser = await _userRepository.UpdateAsync(existingUser);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("NewName", updatedUser.Username);

            var userInDb = await _dbContext.Users.FindAsync(existingUser.UserId);
            Assert.AreEqual("NewName", userInDb.Username);
            Assert.AreEqual("new@example.com", userInDb.Email);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var user = new User { Username = "DeleteMe", Email = "delete@example.com", CreatedAt = DateTime.UtcNow };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var userId = user.UserId;

            // Act
            await _userRepository.DeleteAsync(userId);

            // Assert
            var deletedUser = await _dbContext.Users.FindAsync(userId);
            Assert.IsNull(deletedUser, "User should be removed from DB after DeleteAsync.");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var user1 = new User { Username = "Test1", Email = "test1@example.com", CreatedAt = DateTime.UtcNow };
            var user2 = new User { Username = "Test2", Email = "test2@example.com", CreatedAt = DateTime.UtcNow };
            await _dbContext.Users.AddRangeAsync(user1, user2);
            await _dbContext.SaveChangesAsync();

            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, users.Count(), "Should return all existing users.");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new User { Username = "GetMe", Email = "getme@example.com", CreatedAt = DateTime.UtcNow };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var foundUser = await _userRepository.GetByIdAsync(user.UserId);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(user.UserId, foundUser.UserId);
        }

        [Test]
        public async Task GetByQueryAsync_ShouldReturnMatchingUsers()
        {
            // Arrange
            var user1 = new User { Username = "Alpha", Email = "alpha@example.com", CreatedAt = DateTime.UtcNow };
            var user2 = new User { Username = "Alfred", Email = "alfred@example.com", CreatedAt = DateTime.UtcNow };
            var user3 = new User { Username = "Beta", Email = "beta@example.com", CreatedAt = DateTime.UtcNow };

            await _dbContext.Users.AddRangeAsync(user1, user2, user3);
            await _dbContext.SaveChangesAsync();

            // Act
            var results = await _userRepository.GetByQueryAsync("Al", 10);

            // Assert
            // "Alpha" und "Alfred" matchen "Al%"
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(u => u.Username == "Alpha"));
            Assert.IsTrue(results.Any(u => u.Username == "Alfred"));
        }

        // --------------------------------------------
        // Nicht implementierte Methoden:
        // GetByUsernameAsync, GetByEmailAsync
        // Hier könnte man z.B. prüfen, ob NotImplementedException geworfen wird

        [Test]
        public void GetByUsernameAsync_ShouldThrowNotImplementedException()
        {
            Assert.ThrowsAsync<NotImplementedException>(async () =>
            {
                await _userRepository.GetByUsernameAsync("SomeUser");
            });
        }

        [Test]
        public void GetByEmailAsync_ShouldThrowNotImplementedException()
        {
            Assert.ThrowsAsync<NotImplementedException>(async () =>
            {
                await _userRepository.GetByEmailAsync("email@example.com");
            });
        }
    }
}
