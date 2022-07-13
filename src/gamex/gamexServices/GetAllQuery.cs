namespace gamexServices;

/// <summary>
/// Query to get all items.
/// </summary>
public class GetAllQuery
{
    /// <summary>
    /// Phrase to filter the results
    /// </summary>
    public string SearchPhrase { get; set; }

    /// <summary>
    /// Items page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Size of pages
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Sort by selected column
    /// </summary>
    public string SortBy { get; set; }

    /// <summary>
    /// Direction of sorting
    /// </summary>
    public SortDirection SortDirection { get; set; }
}