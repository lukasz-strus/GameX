namespace gamexServices;

/// <summary>
/// Result of get all request
/// </summary>
/// <typeparam name="T">The type of items in the list</typeparam>
public class GetAllResult<T>
{
    /// <summary>
    /// List of items
    /// </summary>
    public List<T> Items { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// First item on page
    /// </summary>
    public int ItemsFrom { get; set; }

    /// <summary>
    /// Last item on page
    /// </summary>
    public int ItemsTo { get; set; }

    /// <summary>
    /// Total items count
    /// </summary>
    public int TotalItemsCount { get; set; }
}