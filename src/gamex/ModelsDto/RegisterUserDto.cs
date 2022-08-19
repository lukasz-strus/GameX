namespace gamexModels;

public class RegisterUserDto
{
    public string Login { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string Email { get; set; }

    public string ConfirmEmail { get; set; }

    public decimal? Total { get; set; } = 0m;

    public int RoleId { get; set; } = 1;
}