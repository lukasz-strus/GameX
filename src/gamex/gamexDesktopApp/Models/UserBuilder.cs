namespace gamexDesktopApp.Models;

public class UserBuilder
{
    private User _user = new();

    public User Build() => _user;

    public UserBuilder SetId(int id)
    {
        _user.Id = id;
        return this;
    }

    public UserBuilder SetLogin(string login)
    {
        _user.Login = login;
        return this;
    }

    public UserBuilder SetPassword(string password)
    {
        _user.Password = password;
        return this;
    }

    public UserBuilder SetEmial(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder SetTotal(decimal total)
    {
        _user.Total = total;
        return this;
    }

    public UserBuilder SetRole(Role role)
    {
        _user.Role = role;
        return this;
    }
}