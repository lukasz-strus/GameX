namespace gamexModels;

public class UserDto : IDto
{
    public int Id { get; set; }

    public string Login { get; set; }

    public string Email { get; set; }

    public decimal Total { get; set; }

    public int RoleId { get; set; }
}