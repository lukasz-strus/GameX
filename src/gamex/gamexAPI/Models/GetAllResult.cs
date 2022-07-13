namespace gamexAPI.Models;

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

    /// <summary>
    /// Result of get all request
    /// </summary>
    /// <param name="items">List of items</param>
    /// <param name="totalItemsCount">Total items count</param>
    /// <param name="pageSize">Size of pages</param>
    /// <param name="pageNumber">Items page number.</param>
    public GetAllResult(List<T> items, int totalItemsCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
    }
}