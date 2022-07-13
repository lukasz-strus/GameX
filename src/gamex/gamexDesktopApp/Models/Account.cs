namespace gamexDesktopApp.Models;

public class Account
{
    public User AccountHolder { get; set; }
    public string Token { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public decimal Total { get; set; }
    public Role Role { get; set; }
}