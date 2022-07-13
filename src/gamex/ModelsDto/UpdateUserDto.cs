namespace gamexModelsDto;

/// <summary>
/// Data transfer object to update user record
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// User e-mail
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User e-mail
    /// </summary>
    public string ConfirmEmail { get; set; }

    /// <summary>
    /// The sum of the user's money
    /// </summary>
    public decimal? Total { get; set; }

    /// <summary>
    /// The ID number of user role
    /// </summary>
    public int? RoleId { get; set; }
}