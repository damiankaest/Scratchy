using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Scratchy.Domain.Configuration;

namespace Scratchy.Persistence.DB
{
    /// <summary>
    /// MongoDB database context providing access to collections and database operations
    /// </summary>
    public class MongoDbContext : IDisposable
    {
        private readonly MongoDBSettings _settings;
        private readonly ILogger<MongoDbContext> _logger;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private bool _disposed = false;

        public MongoDbContext(IOptions<MongoDBSettings> settings, ILogger<MongoDbContext> logger)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (!_settings.IsValid())
            {
                throw new InvalidOperationException("MongoDB settings are not valid. Please check your configuration.");
            }

            try
            {
                // Create MongoDB client with configured settings
                var mongoClientSettings = CreateMongoClientSettings();
                _mongoClient = new MongoClient(mongoClientSettings);
                
                // Get database with environment-specific name
                var databaseName = _settings.GetEnvironmentDatabaseName();
                _database = _mongoClient.GetDatabase(databaseName);

                _logger.LogInformation("MongoDB context initialized successfully. Database: {DatabaseName}, Environment: {Environment}", 
                    databaseName, _settings.Environment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize MongoDB context");
                throw;
            }
        }

        /// <summary>
        /// Gets the MongoDB database instance
        /// </summary>
        public IMongoDatabase Database => _database;

        /// <summary>
        /// Gets the MongoDB client instance
        /// </summary>
        public IMongoClient Client => _mongoClient;

        /// <summary>
        /// Gets the current database name
        /// </summary>
        public string DatabaseName => _database.DatabaseNamespace.DatabaseName;

        /// <summary>
        /// Gets a collection of the specified type
        /// </summary>
        /// <typeparam name="T">The document type</typeparam>
        /// <param name="collectionName">The collection name</param>
        /// <returns>The MongoDB collection</returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("Collection name cannot be null or empty", nameof(collectionName));

            return _database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Gets a collection of the specified document type using automatic collection naming
        /// </summary>
        /// <typeparam name="T">The document type</typeparam>
        /// <returns>The MongoDB collection</returns>
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = GetCollectionName<T>();
            return GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Gets the collection name for a document type
        /// </summary>
        /// <typeparam name="T">The document type</typeparam>
        /// <returns>Collection name</returns>
        private string GetCollectionName<T>()
        {
            var typeName = typeof(T).Name;
            
            // Remove "Document" suffix if present and convert to plural lowercase
            if (typeName.EndsWith("Document"))
            {
                typeName = typeName.Substring(0, typeName.Length - 8);
            }

            // Convert to plural and lowercase for collection naming convention
            return typeName.ToLowerInvariant() switch
            {
                "user" => "users",
                "artist" => "artists", 
                "album" => "albums",
                "track" => "tracks",
                "post" => "posts",
                "comment" => "comments",
                "scratch" => "scratches",
                "playlist" => "playlists",
                "follow" => "follows",
                "notification" => "notifications",
                "badge" => "badges",
                "tag" => "tags",
                "genre" => "genres",
                _ => $"{typeName.ToLowerInvariant()}s"
            };
        }

        /// <summary>
        /// Creates indexes for all collections on startup
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task InitializeIndexesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting MongoDB index initialization...");

                // Initialize indexes for each collection
                await CreateUserIndexesAsync(cancellationToken);
                await CreateArtistIndexesAsync(cancellationToken);
                await CreateAlbumIndexesAsync(cancellationToken);
                await CreatePostIndexesAsync(cancellationToken);
                await CreateScratchIndexesAsync(cancellationToken);
                await CreatePlaylistIndexesAsync(cancellationToken);
                await CreateFollowIndexesAsync(cancellationToken);
                await CreateNotificationIndexesAsync(cancellationToken);
                await CreateBadgeIndexesAsync(cancellationToken);

                _logger.LogInformation("MongoDB index initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize MongoDB indexes");
                throw;
            }
        }

        /// <summary>
        /// Checks if the database connection is healthy
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if connection is healthy, false otherwise</returns>
        public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Simple ping to check connection
                await _database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}", 
                    cancellationToken: cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "MongoDB health check failed");
                return false;
            }
        }

        /// <summary>
        /// Gets database statistics
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Database stats</returns>
        public async Task<MongoDB.Bson.BsonDocument> GetDatabaseStatsAsync(CancellationToken cancellationToken = default)
        {
            return await _database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{dbStats:1}", 
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Starts a new session for transactions
        /// </summary>
        /// <param name="options">Session options</param>
        /// <returns>Client session</returns>
        public IClientSessionHandle StartSession(ClientSessionOptions? options = null)
        {
            return _mongoClient.StartSession(options);
        }

        /// <summary>
        /// Creates MongoDB client settings based on configuration
        /// </summary>
        /// <returns>Configured MongoClientSettings</returns>
        private MongoClientSettings CreateMongoClientSettings()
        {
            var connectionString = _settings.GetOptimizedConnectionString();
            var clientSettings = MongoClientSettings.FromConnectionString(connectionString);

            // Apply configuration settings
            clientSettings.ConnectTimeout = _settings.ConnectionTimeout;
            clientSettings.SocketTimeout = _settings.SocketTimeout;
            clientSettings.ServerSelectionTimeout = _settings.ServerSelectionTimeout;
            clientSettings.MaxConnectionPoolSize = _settings.MaxConnectionPoolSize;
            clientSettings.MinConnectionPoolSize = _settings.MinConnectionPoolSize;
            clientSettings.MaxConnectionIdleTime = _settings.MaxConnectionIdleTime;
            clientSettings.WaitQueueTimeout = _settings.WaitQueueTimeout;
            clientSettings.RetryWrites = _settings.EnableRetryWrites;
            clientSettings.RetryReads = _settings.EnableRetryReads;

            // Set application name for monitoring
            clientSettings.ApplicationName = $"Scratchy-{_settings.Environment}";

            return clientSettings;
        }

        #region Index Creation Methods

        private async Task CreateUserIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("users");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("firebaseId"), 
                    new CreateIndexOptions { Unique = true, Name = "firebaseId_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("username"), 
                    new CreateIndexOptions { Unique = true, Name = "username_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("email"), 
                    new CreateIndexOptions { Unique = true, Sparse = true, Name = "email_unique_sparse" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("username"), 
                    new CreateIndexOptions { Name = "username_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt"), 
                    new CreateIndexOptions { Name = "createdAt_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("stats.followersCount"), 
                    new CreateIndexOptions { Name = "followersCount_desc" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateArtistIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("artists");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("spotifyId"), 
                    new CreateIndexOptions { Unique = true, Name = "spotifyId_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("name"), 
                    new CreateIndexOptions { Name = "name_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("name"), 
                    new CreateIndexOptions { Name = "name_asc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("stats.followersCount"), 
                    new CreateIndexOptions { Name = "artistFollowersCount_desc" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateAlbumIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("albums");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("spotifyId"), 
                    new CreateIndexOptions { Unique = true, Sparse = true, Name = "album_spotifyId_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("artistId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("releaseDate")), 
                    new CreateIndexOptions { Name = "artistId_releaseDate" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("title").Text("artistName"), 
                    new CreateIndexOptions { Name = "album_title_artist_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("genres"), 
                    new CreateIndexOptions { Name = "genres_multi" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("releaseDate"), 
                    new CreateIndexOptions { Name = "releaseDate_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("stats.averageRating"), 
                    new CreateIndexOptions { Name = "averageRating_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("nfcTagId"), 
                    new CreateIndexOptions { Sparse = true, Name = "nfcTagId_sparse" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("tracks._id"), 
                    new CreateIndexOptions { Name = "tracks_id" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreatePostIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("posts");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("userId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "userId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("albumId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "albumId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt"), 
                    new CreateIndexOptions { Name = "posts_createdAt_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("content"), 
                    new CreateIndexOptions { Name = "content_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("rating"), 
                    new CreateIndexOptions { Name = "rating_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("stats.likesCount"), 
                    new CreateIndexOptions { Name = "likesCount_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("comments.userId"), 
                    new CreateIndexOptions { Name = "comments_userId" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateScratchIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("scratches");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("userId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "scratch_userId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("albumId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "scratch_albumId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("tags"), 
                    new CreateIndexOptions { Name = "tags_multi" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("title").Text("content"), 
                    new CreateIndexOptions { Name = "scratch_title_content_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("likeCounter"), 
                    new CreateIndexOptions { Name = "likeCounter_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("rating"), 
                    new CreateIndexOptions { Name = "scratch_rating_desc" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt"), 
                    new CreateIndexOptions { Name = "scratch_createdAt_desc" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreatePlaylistIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("playlists");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("userId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "playlist_userId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("isPublic"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("stats.followersCount")), 
                    new CreateIndexOptions { Name = "isPublic_followersCount" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Text("title").Text("description"), 
                    new CreateIndexOptions { Name = "playlist_title_desc_text" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("tracks.trackId"), 
                    new CreateIndexOptions { Name = "playlist_tracks_trackId" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("tracks.albumId"), 
                    new CreateIndexOptions { Name = "playlist_tracks_albumId" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt"), 
                    new CreateIndexOptions { Name = "playlist_createdAt_desc" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateFollowIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("follows");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followerId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "followerId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followedId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "followedId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followerId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followedId")), 
                    new CreateIndexOptions { Unique = true, Name = "follow_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followedId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("followerId")), 
                    new CreateIndexOptions { Name = "followedId_followerId" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateNotificationIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("notifications");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("userId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("isRead"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "userId_isRead_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("expiresAt"), 
                    new CreateIndexOptions { ExpireAfter = TimeSpan.Zero, Name = "expiresAt_ttl" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("senderId"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "senderId_createdAt" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("type"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("createdAt")), 
                    new CreateIndexOptions { Name = "type_createdAt" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        private async Task CreateBadgeIndexesAsync(CancellationToken cancellationToken)
        {
            var collection = GetCollection<MongoDB.Bson.BsonDocument>("badges");
            var indexKeysDefinitions = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>
            {
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("name"), 
                    new CreateIndexOptions { Unique = true, Name = "badge_name_unique" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Combine(
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("category"),
                    Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("rarity")), 
                    new CreateIndexOptions { Name = "category_rarity" }),
                new(Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("isActive"), 
                    new CreateIndexOptions { Name = "isActive" })
            };

            await collection.Indexes.CreateManyAsync(indexKeysDefinitions, cancellationToken);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // MongoDB client is thread-safe and handles its own cleanup
                _logger.LogInformation("MongoDB context disposed");
                _disposed = true;
            }
        }

        #endregion
    }
}
