namespace gamexModelsDto;

/// <summary>
/// Data transfer object to show game record
/// </summary>
public class GameDto
{
    /// <summary>
    /// The ID number of the game
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the game
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the game
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The price of the game
    /// </summary>
    public decimal Price { get; set; }
}