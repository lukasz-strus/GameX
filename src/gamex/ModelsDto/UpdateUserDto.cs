namespace gamexModels;

public class UpdateUserDto
{
    public string Login { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string Email { get; set; }

    public string ConfirmEmail { get; set; }

    public decimal? Total { get; set; }

    public int? RoleId { get; set; }
}