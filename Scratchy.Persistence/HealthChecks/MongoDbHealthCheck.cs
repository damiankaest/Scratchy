using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.HealthChecks
{
    /// <summary>
    /// Health check for MongoDB connection and database operations
    /// </summary>
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ILogger<MongoDbHealthCheck> _logger;

        public MongoDbHealthCheck(MongoDbContext mongoDbContext, ILogger<MongoDbHealthCheck> logger)
        {
            _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if MongoDB connection is healthy
                var isHealthy = await _mongoDbContext.IsHealthyAsync(cancellationToken);
                
                if (!isHealthy)
                {
                    _logger.LogWarning("MongoDB health check failed - connection is not healthy");
                    return HealthCheckResult.Unhealthy("MongoDB connection is not healthy");
                }

                // Get basic database stats for additional health information
                var stats = await _mongoDbContext.GetDatabaseStatsAsync(cancellationToken);
                var data = new Dictionary<string, object>
                {
                    { "database", _mongoDbContext.DatabaseName },
                    { "connection_status", "healthy" },
                    { "collections", stats.GetValue("collections", 0).AsInt32 },
                    { "indexes", stats.GetValue("indexes", 0).AsInt32 },
                    { "data_size", FormatBytes(stats.GetValue("dataSize", 0).AsInt64) },
                    { "storage_size", FormatBytes(stats.GetValue("storageSize", 0).AsInt64) }
                };

                _logger.LogDebug("MongoDB health check passed. Database: {Database}, Collections: {Collections}", 
                    _mongoDbContext.DatabaseName, stats.GetValue("collections", 0).AsInt32);

                return HealthCheckResult.Healthy("MongoDB is healthy", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MongoDB health check failed with exception");
                return HealthCheckResult.Unhealthy("MongoDB health check failed", ex);
            }
        }

        private static string FormatBytes(long bytes)
        {
            const long scale = 1024;
            string[] orders = { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}
