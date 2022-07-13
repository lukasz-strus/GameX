namespace gamexModelsDto;

/// <summary>
/// Data transfer object to show user record
/// </summary>
public class UserDto
{
    /// <summary>
    /// The ID number of the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User e-mail
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The sum of the user's money
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// The ID number of user role
    /// </summary>
    public int RoleId { get; set; }
}