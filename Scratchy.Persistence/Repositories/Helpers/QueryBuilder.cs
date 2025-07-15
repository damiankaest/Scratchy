using MongoDB.Bson;
using MongoDB.Driver;
using Scratchy.Domain.Models;
using System.Linq.Expressions;

namespace Scratchy.Persistence.Repositories.Helpers;

/// <summary>
/// Query builder helpers for MongoDB operations
/// </summary>
public static class QueryBuilder
{
    /// <summary>
    /// Creates a filter for text search across multiple fields
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="searchTerm">Search term</param>
    /// <param name="fields">Fields to search in</param>
    /// <returns>Filter definition for text search</returns>
    public static FilterDefinition<T> CreateTextSearchFilter<T>(string searchTerm, params Expression<Func<T, object>>[] fields) where T : BaseDocument
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || fields == null || !fields.Any())
            return FilterDefinition<T>.Empty;

        var filterBuilder = Builders<T>.Filter;
        var filters = new List<FilterDefinition<T>>();

        foreach (var field in fields)
        {
            // Create regex filter for partial matching
            var regexFilter = filterBuilder.Regex(new ExpressionFieldDefinition<T, object>(field), new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
            filters.Add(regexFilter);
        }

        return filterBuilder.Or(filters);
    }

    /// <summary>
    /// Creates a filter for date range queries
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="dateField">Date field expression</param>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <returns>Filter definition for date range</returns>
    public static FilterDefinition<T> CreateDateRangeFilter<T>(
        Expression<Func<T, DateTime>> dateField,
        DateTime? startDate = null,
        DateTime? endDate = null) where T : BaseDocument
    {
        var filterBuilder = Builders<T>.Filter;
        var filters = new List<FilterDefinition<T>>();

        if (startDate.HasValue)
        {
            filters.Add(filterBuilder.Gte(dateField, startDate.Value));
        }

        if (endDate.HasValue)
        {
            filters.Add(filterBuilder.Lte(dateField, endDate.Value));
        }

        return filters.Any() ? filterBuilder.And(filters) : FilterDefinition<T>.Empty;
    }

    /// <summary>
    /// Creates a filter for "in" operations with ObjectId arrays
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="field">Field expression</param>
    /// <param name="values">Values to match</param>
    /// <returns>Filter definition for "in" operation</returns>
    public static FilterDefinition<T> CreateInFilter<T>(
        Expression<Func<T, string>> field,
        IEnumerable<string> values) where T : BaseDocument
    {
        if (values == null || !values.Any())
            return FilterDefinition<T>.Empty;

        var filterBuilder = Builders<T>.Filter;
        return filterBuilder.In(field, values);
    }

    /// <summary>
    /// Creates a case-insensitive equality filter
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="field">Field expression</param>
    /// <param name="value">Value to match</param>
    /// <returns>Filter definition for case-insensitive equality</returns>
    public static FilterDefinition<T> CreateCaseInsensitiveFilter<T>(
        Expression<Func<T, string>> field,
        string value) where T : BaseDocument
    {
        if (string.IsNullOrWhiteSpace(value))
            return FilterDefinition<T>.Empty;

        var filterBuilder = Builders<T>.Filter;
        // Escape special regex characters manually
        var escapedValue = EscapeRegexSpecialCharacters(value);
        var regexOptions = new BsonRegularExpression(escapedValue, "i");
        return filterBuilder.Regex(new ExpressionFieldDefinition<T, string>(field), regexOptions);
    }

    /// <summary>
    /// Escapes special regex characters in a string
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Escaped string</returns>
    private static string EscapeRegexSpecialCharacters(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Escape special regex characters
        var specialChars = new char[] { '\\', '^', '$', '.', '|', '?', '*', '+', '(', ')', '[', ']', '{', '}' };
        foreach (var c in specialChars)
        {
            input = input.Replace(c.ToString(), "\\" + c);
        }
        return input;
    }

    /// <summary>
    /// Creates a compound sort definition
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="sortFields">Array of sort field definitions</param>
    /// <returns>Compound sort definition</returns>
    public static SortDefinition<T> CreateCompoundSort<T>(params (Expression<Func<T, object>> field, bool ascending)[] sortFields) where T : BaseDocument
    {
        if (sortFields == null || !sortFields.Any())
            return Builders<T>.Sort.Descending(x => x.CreatedAt); // Default sort

        var sortBuilder = Builders<T>.Sort;
        var sorts = new List<SortDefinition<T>>();

        foreach (var (field, ascending) in sortFields)
        {
            sorts.Add(ascending ? sortBuilder.Ascending(field) : sortBuilder.Descending(field));
        }

        return sortBuilder.Combine(sorts);
    }

    /// <summary>
    /// Creates an update definition for incrementing numeric fields
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <param name="field">Numeric field expression</param>
    /// <param name="increment">Increment value (default: 1)</param>
    /// <returns>Update definition for increment operation</returns>
    public static UpdateDefinition<T> CreateIncrementUpdate<T>(
        Expression<Func<T, int>> field,
        int increment = 1) where T : BaseDocument
    {
        var updateBuilder = Builders<T>.Update;
        return updateBuilder
            .Inc(field, increment)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
    }

    /// <summary>
    /// Creates an update definition for adding items to an array
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <typeparam name="TItem">Array item type</typeparam>
    /// <param name="arrayField">Array field expression</param>
    /// <param name="items">Items to add</param>
    /// <returns>Update definition for array addition</returns>
    public static UpdateDefinition<T> CreateArrayAddUpdate<T, TItem>(
        Expression<Func<T, IEnumerable<TItem>>> arrayField,
        params TItem[] items) where T : BaseDocument
    {
        var updateBuilder = Builders<T>.Update;
        return updateBuilder
            .AddToSetEach(arrayField, items)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
    }

    /// <summary>
    /// Creates an update definition for removing items from an array
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    /// <typeparam name="TItem">Array item type</typeparam>
    /// <param name="arrayField">Array field expression</param>
    /// <param name="items">Items to remove</param>
    /// <returns>Update definition for array removal</returns>
    public static UpdateDefinition<T> CreateArrayRemoveUpdate<T, TItem>(
        Expression<Func<T, IEnumerable<TItem>>> arrayField,
        params TItem[] items) where T : BaseDocument
    {
        var updateBuilder = Builders<T>.Update;
        return updateBuilder
            .PullAll(arrayField, items)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
    }
}
