namespace Scratchy.Domain.Enum;

/// <summary>
/// Enum for specifying sort order in queries
/// </summary>
public enum SortOrder
{
    /// <summary>
    /// Ascending sort order (A-Z, 0-9, earliest to latest)
    /// </summary>
    Ascending = 1,

    /// <summary>
    /// Descending sort order (Z-A, 9-0, latest to earliest)
    /// </summary>
    Descending = -1
}
