namespace gamexEntities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public decimal Total { get; set; }

    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}