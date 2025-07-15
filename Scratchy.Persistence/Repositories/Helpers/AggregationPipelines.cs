using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Scratchy.Domain.Models;

namespace Scratchy.Persistence.Repositories.Helpers;

/// <summary>
/// Pre-built aggregation pipelines for common operations
/// </summary>
public static class AggregationPipelines
{
    /// <summary>
    /// Creates a pipeline for paginated results with count
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="matchFilter">Match filter</param>
    /// <param name="sortDefinition">Sort definition</param>
    /// <param name="skip">Number of documents to skip</param>
    /// <param name="limit">Number of documents to take</param>
    /// <returns>Pipeline for paginated results with total count</returns>
    public static PipelineDefinition<T, PaginatedResult<T>> CreatePaginationPipeline<T>(
        FilterDefinition<T>? matchFilter = null,
        SortDefinition<T>? sortDefinition = null,
        int skip = 0,
        int limit = 50) where T : BaseDocument
    {
        var pipeline = new List<BsonDocument>();

        // Add match stage if filter provided
        if (matchFilter != null)
        {
            pipeline.Add(new BsonDocument("$match", matchFilter.Render(BsonSerializer.SerializerRegistry.GetSerializer<T>(), BsonSerializer.SerializerRegistry)));
        }

        // Add facet stage for pagination with count
        var facetDoc = new BsonDocument
        {
            {
                "$facet", new BsonDocument
                {
                    {
                        "data", new BsonArray
                        {
                            // Sort stage
                            sortDefinition != null 
                                ? new BsonDocument("$sort", sortDefinition.Render(BsonSerializer.SerializerRegistry.GetSerializer<T>(), BsonSerializer.SerializerRegistry))
                                : new BsonDocument("$sort", new BsonDocument("createdAt", -1)),
                            new BsonDocument("$skip", skip),
                            new BsonDocument("$limit", limit)
                        }
                    },
                    {
                        "count", new BsonArray
                        {
                            new BsonDocument("$count", "total")
                        }
                    }
                }
            }
        };
        pipeline.Add(facetDoc);

        // Project the final result
        pipeline.Add(new BsonDocument
        {
            {
                "$project", new BsonDocument
                {
                    { "data", 1 },
                    { "totalCount", new BsonDocument("$arrayElemAt", new BsonArray { "$count.total", 0 }) },
                    { "page", skip / limit + 1 },
                    { "pageSize", limit },
                    { "hasNextPage", new BsonDocument("$gt", new BsonArray { new BsonDocument("$arrayElemAt", new BsonArray { "$count.total", 0 }), skip + limit }) }
                }
            }
        });

        return PipelineDefinition<T, PaginatedResult<T>>.Create(pipeline);
    }

    /// <summary>
    /// Creates a pipeline for grouping and counting by a specific field
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="groupByField">Field to group by</param>
    /// <param name="matchFilter">Optional match filter</param>
    /// <returns>Pipeline for group by count operation</returns>
    public static PipelineDefinition<T, GroupByResult> CreateGroupByCountPipeline<T>(
        string groupByField,
        FilterDefinition<T>? matchFilter = null) where T : BaseDocument
    {
        var pipeline = new List<BsonDocument>();

        // Add match stage if filter provided
        if (matchFilter != null)
        {
            pipeline.Add(new BsonDocument("$match", matchFilter.Render(BsonSerializer.SerializerRegistry.GetSerializer<T>(), BsonSerializer.SerializerRegistry)));
        }

        // Group by field and count
        pipeline.Add(new BsonDocument
        {
            {
                "$group", new BsonDocument
                {
                    { "_id", $"${groupByField}" },
                    { "count", new BsonDocument("$sum", 1) }
                }
            }
        });

        // Sort by count descending
        pipeline.Add(new BsonDocument("$sort", new BsonDocument("count", -1)));

        // Project to final structure
        pipeline.Add(new BsonDocument
        {
            {
                "$project", new BsonDocument
                {
                    { "_id", 0 },
                    { "value", "$_id" },
                    { "count", 1 }
                }
            }
        });

        return PipelineDefinition<T, GroupByResult>.Create(pipeline);
    }

    /// <summary>
    /// Creates a pipeline for time-based aggregation (daily, weekly, monthly)
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="dateField">Date field to group by</param>
    /// <param name="groupBy">Time grouping (day, week, month, year)</param>
    /// <param name="matchFilter">Optional match filter</param>
    /// <returns>Pipeline for time-based aggregation</returns>
    public static PipelineDefinition<T, TimeBasedResult> CreateTimeBasedAggregationPipeline<T>(
        string dateField,
        TimeGrouping groupBy,
        FilterDefinition<T>? matchFilter = null) where T : BaseDocument
    {
        var pipeline = new List<BsonDocument>();

        // Add match stage if filter provided
        if (matchFilter != null)
        {
            pipeline.Add(new BsonDocument("$match", matchFilter.Render(BsonSerializer.SerializerRegistry.GetSerializer<T>(), BsonSerializer.SerializerRegistry)));
        }

        // Create date grouping based on the specified period
        var dateGrouping = groupBy switch
        {
            TimeGrouping.Day => new BsonDocument
            {
                { "year", new BsonDocument("$year", $"${dateField}") },
                { "month", new BsonDocument("$month", $"${dateField}") },
                { "day", new BsonDocument("$dayOfMonth", $"${dateField}") }
            },
            TimeGrouping.Week => new BsonDocument
            {
                { "year", new BsonDocument("$year", $"${dateField}") },
                { "week", new BsonDocument("$week", $"${dateField}") }
            },
            TimeGrouping.Month => new BsonDocument
            {
                { "year", new BsonDocument("$year", $"${dateField}") },
                { "month", new BsonDocument("$month", $"${dateField}") }
            },
            TimeGrouping.Year => new BsonDocument
            {
                { "year", new BsonDocument("$year", $"${dateField}") }
            },
            _ => throw new ArgumentException($"Unsupported time grouping: {groupBy}")
        };

        // Group by date period and count
        pipeline.Add(new BsonDocument
        {
            {
                "$group", new BsonDocument
                {
                    { "_id", dateGrouping },
                    { "count", new BsonDocument("$sum", 1) }
                }
            }
        });

        // Sort by date
        pipeline.Add(new BsonDocument("$sort", new BsonDocument("_id", 1)));

        // Project to final structure
        pipeline.Add(new BsonDocument
        {
            {
                "$project", new BsonDocument
                {
                    { "_id", 0 },
                    { "period", "$_id" },
                    { "count", 1 }
                }
            }
        });

        return PipelineDefinition<T, TimeBasedResult>.Create(pipeline);
    }

    /// <summary>
    /// Creates a pipeline for search with highlighting and ranking
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="searchTerm">Search term</param>
    /// <param name="searchFields">Fields to search in</param>
    /// <param name="limit">Number of results to return</param>
    /// <returns>Pipeline for text search with scoring</returns>
    public static PipelineDefinition<T, SearchResult<T>> CreateTextSearchPipeline<T>(
        string searchTerm,
        string[] searchFields,
        int limit = 20) where T : BaseDocument
    {
        var pipeline = new List<BsonDocument>();

        // Add text search stage
        pipeline.Add(new BsonDocument
        {
            {
                "$match", new BsonDocument
                {
                    {
                        "$text", new BsonDocument
                        {
                            { "$search", searchTerm }
                        }
                    }
                }
            }
        });

        // Add score and sort by relevance
        pipeline.Add(new BsonDocument
        {
            {
                "$addFields", new BsonDocument
                {
                    { "score", new BsonDocument("$meta", "textScore") }
                }
            }
        });

        pipeline.Add(new BsonDocument("$sort", new BsonDocument("score", new BsonDocument("$meta", "textScore"))));
        pipeline.Add(new BsonDocument("$limit", limit));

        // Project to final structure
        pipeline.Add(new BsonDocument
        {
            {
                "$project", new BsonDocument
                {
                    { "document", "$$ROOT" },
                    { "score", 1 },
                    { "_id", 0 }
                }
            }
        });

        return PipelineDefinition<T, SearchResult<T>>.Create(pipeline);
    }
}

/// <summary>
/// Result structure for paginated queries
/// </summary>
/// <typeparam name="T">Document type</typeparam>
public class PaginatedResult<T>
{
    public List<T> Data { get; set; } = new();
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
}

/// <summary>
/// Result structure for group by operations
/// </summary>
public class GroupByResult
{
    public string Value { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// Result structure for time-based aggregations
/// </summary>
public class TimeBasedResult
{
    public object Period { get; set; } = new();
    public int Count { get; set; }
}

/// <summary>
/// Result structure for text search operations
/// </summary>
/// <typeparam name="T">Document type</typeparam>
public class SearchResult<T>
{
    public T Document { get; set; } = default!;
    public double Score { get; set; }
}

/// <summary>
/// Time grouping options for aggregation
/// </summary>
public enum TimeGrouping
{
    Day,
    Week,
    Month,
    Year
}
