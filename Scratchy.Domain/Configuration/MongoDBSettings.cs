using System.ComponentModel.DataAnnotations;

namespace Scratchy.Domain.Configuration
{
    /// <summary>
    /// MongoDB configuration settings
    /// </summary>
    public class MongoDBSettings
    {
        public const string SectionName = "MongoDB";

        /// <summary>
        /// MongoDB connection string
        /// </summary>
        [Required]
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Database name to connect to
        /// </summary>
        [Required]
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Connection timeout duration
        /// </summary>
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Socket timeout duration
        /// </summary>
        public TimeSpan SocketTimeout { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Server selection timeout duration
        /// </summary>
        public TimeSpan ServerSelectionTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Maximum number of connections in the connection pool
        /// </summary>
        [Range(1, 1000)]
        public int MaxConnectionPoolSize { get; set; } = 100;

        /// <summary>
        /// Minimum number of connections in the connection pool
        /// </summary>
        [Range(0, 100)]
        public int MinConnectionPoolSize { get; set; } = 5;

        /// <summary>
        /// Maximum time a connection can remain idle in the pool
        /// </summary>
        public TimeSpan MaxConnectionIdleTime { get; set; } = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Maximum time to wait for a connection from the pool
        /// </summary>
        public TimeSpan WaitQueueTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Enable retry writes for supported operations
        /// </summary>
        public bool EnableRetryWrites { get; set; } = true;

        /// <summary>
        /// Enable retry reads for supported operations
        /// </summary>
        public bool EnableRetryReads { get; set; } = true;

        /// <summary>
        /// Number of retry attempts for failed operations
        /// </summary>
        [Range(0, 10)]
        public int RetryAttempts { get; set; } = 3;

        /// <summary>
        /// Delay between retry attempts
        /// </summary>
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Current environment (Development, Staging, Production)
        /// </summary>
        public string Environment { get; set; } = "Development";

        /// <summary>
        /// Validate the MongoDB settings
        /// </summary>
        /// <returns>True if settings are valid, false otherwise</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return false;

            if (string.IsNullOrWhiteSpace(DatabaseName))
                return false;

            if (MaxConnectionPoolSize <= 0 || MinConnectionPoolSize < 0)
                return false;

            if (MaxConnectionPoolSize < MinConnectionPoolSize)
                return false;

            if (RetryAttempts < 0)
                return false;

            return true;
        }

        /// <summary>
        /// Get environment-specific database name
        /// </summary>
        /// <returns>Database name with environment suffix if needed</returns>
        public string GetEnvironmentDatabaseName()
        {
            return Environment?.ToLower() switch
            {
                "development" => $"{DatabaseName}Dev",
                "staging" => $"{DatabaseName}Stage",
                "production" => DatabaseName,
                _ => $"{DatabaseName}Dev"
            };
        }

        /// <summary>
        /// Get connection string with environment-specific optimizations
        /// </summary>
        /// <returns>Optimized connection string for the current environment</returns>
        public string GetOptimizedConnectionString()
        {
            var connectionString = ConnectionString;

            // Add environment-specific connection string parameters
            if (Environment?.ToLower() == "development")
            {
                // Development optimizations: faster timeouts, smaller pool
                if (!connectionString.Contains("connectTimeoutMS"))
                    connectionString += $"&connectTimeoutMS={ConnectionTimeout.TotalMilliseconds}";
                
                if (!connectionString.Contains("maxPoolSize"))
                    connectionString += $"&maxPoolSize={Math.Min(MaxConnectionPoolSize, 25)}";
            }
            else if (Environment?.ToLower() == "production")
            {
                // Production optimizations: longer timeouts, larger pool
                if (!connectionString.Contains("connectTimeoutMS"))
                    connectionString += $"&connectTimeoutMS={ConnectionTimeout.TotalMilliseconds}";
                
                if (!connectionString.Contains("maxPoolSize"))
                    connectionString += $"&maxPoolSize={MaxConnectionPoolSize}";
                
                if (!connectionString.Contains("minPoolSize"))
                    connectionString += $"&minPoolSize={MinConnectionPoolSize}";
            }

            return connectionString;
        }
    }
}
