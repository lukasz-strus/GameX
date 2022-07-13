namespace gamexModelsDto;

/// <summary>
/// Data transfer object to create new user record
/// </summary>
public class RegisterUserDto
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
    public decimal? Total { get; set; } = 0m;

    /// <summary>
    /// The ID number of user role
    /// </summary>
    public int RoleId { get; set; } = 1;
}