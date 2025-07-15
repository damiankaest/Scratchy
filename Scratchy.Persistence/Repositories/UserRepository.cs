using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Models;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class UserRepository : MongoRepository<UserDocument>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(MongoDbContext context, ILogger<UserRepository> logger) 
            : base(context, logger)
        {
            _logger = logger;
        }

        #region IUserRepository Implementation

        /// <summary>
        /// Search users by query (username, email)
        /// </summary>
        public async Task<IEnumerable<UserDocument>> GetByQueryAsync(string query, int limit)
        {
            try
            {
                _logger.LogInformation("Searching users with query: {Query}, limit: {Limit}", query, limit);

                var filter = Builders<UserDocument>.Filter.Or(
                    Builders<UserDocument>.Filter.Regex(u => u.Username, new BsonRegularExpression(query, "i")),
                    Builders<UserDocument>.Filter.Regex(u => u.Email, new BsonRegularExpression(query, "i"))
                );

                var users = await _collection
                    .Find(filter)
                    .Limit(limit)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} users matching query: {Query}", users.Count, query);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users with query: {Query}", query);
                throw;
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<IEnumerable<UserDocument>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                return await base.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        /// <summary>
        /// Get user by Firebase ID
        /// </summary>
        public async Task<UserDocument> GetByFirebaseIdAsync(string firebaseId)
        {
            try
            {
                _logger.LogInformation("Getting user by Firebase ID: {FirebaseId}", firebaseId);

                var filter = Builders<UserDocument>.Filter.Eq(u => u.FirebaseId, firebaseId);
                var user = await _collection.Find(filter).FirstOrDefaultAsync();

                if (user != null)
                {
                    _logger.LogInformation("Found user with Firebase ID: {FirebaseId}", firebaseId);
                }
                else
                {
                    _logger.LogWarning("No user found with Firebase ID: {FirebaseId}", firebaseId);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by Firebase ID: {FirebaseId}", firebaseId);
                throw;
            }
        }

        /// <summary>
        /// Add new user
        /// </summary>
        public async Task<UserDocument> AddAsync(UserDocument user)
        {
            try
            {
                _logger.LogInformation("Adding new user: {Username}", user.Username);
                return await base.CreateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user: {Username}", user.Username);
                throw;
            }
        }

        /// <summary>
        /// Update existing user
        /// </summary>
        public async Task<UserDocument> UpdateAsync(UserDocument user)
        {
            try
            {
                _logger.LogInformation("Updating user: {Username}", user.Username);
                return await base.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {Username}", user.Username);
                throw;
            }
        }

        /// <summary>
        /// Delete user by ID
        /// </summary>
        public async Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {Id}", id);
                await base.DeleteAsync(new ObjectId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Get users by username
        /// </summary>
        public async Task<List<UserDocument>> GetByUsernameAsync(string username)
        {
            try
            {
                _logger.LogInformation("Getting users by username: {Username}", username);

                var filter = Builders<UserDocument>.Filter.Eq(u => u.Username, username);
                var users = await _collection.Find(filter).ToListAsync();

                _logger.LogInformation("Found {Count} users with username: {Username}", users.Count, username);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by username: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        public async Task<UserDocument> GetByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Getting user by email: {Email}", email);

                var filter = Builders<UserDocument>.Filter.Eq(u => u.Email, email);
                var user = await _collection.Find(filter).FirstOrDefaultAsync();

                if (user != null)
                {
                    _logger.LogInformation("Found user with email: {Email}", email);
                }
                else
                {
                    _logger.LogWarning("No user found with email: {Email}", email);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<UserDocument> GetByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation("Getting user by ID: {Id}", id);
                return await base.GetByIdAsync(new ObjectId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Get user profile with aggregated data
        /// </summary>
        public async Task<UserProfileDto> GetUserProfileByIdAsync(string userId, string currentUserId)
        {
            try
            {
                _logger.LogInformation("Getting user profile for UserId: {UserId}, CurrentUser: {CurrentUserId}", userId, currentUserId);

                var user = await GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId);
                    return null;
                }

                // TODO: Add aggregation logic for follower counts, posts, etc.
                // This would require additional repositories or aggregation pipelines

                var userProfile = new UserProfileDto
                {
                    UserId = userId,
                    UserName = user.Username,
                    UserImageUrl = user.ProfilePictureUrl,
                    // Add other profile fields as needed
                };

                _logger.LogInformation("Successfully created user profile for UserId: {UserId}", userId);
                return userProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for UserId: {UserId}", userId);
                throw;
            }
        }

        #endregion

        #region Additional Helper Methods

        /// <summary>
        /// Check if username exists
        /// </summary>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Username, username);
                var count = await _collection.CountDocumentsAsync(filter);
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if username exists: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Email, email);
                var count = await _collection.CountDocumentsAsync(filter);
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists: {Email}", email);
                throw;
            }
        }

        #endregion
    }
}
