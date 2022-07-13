namespace gamexServices;

public class GetAllResult<T>
{
    public List<T> Items { get; set; }

    public int TotalPages { get; set; }

    public int ItemsFrom { get; set; }

    public int ItemsTo { get; set; }

    public int TotalItemsCount { get; set; }
}