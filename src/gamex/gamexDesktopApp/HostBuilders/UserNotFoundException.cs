namespace gamexDesktopApp.Exceptions;

public class UserNotFoundException : Exception
{
    public string Login { get; set; }

    public UserNotFoundException(string login)
    {
        Login = login;
    }

    public UserNotFoundException(string message, string login) : base(message)
    {
        Login = login;
    }

    public UserNotFoundException(string message, Exception innerException, string login) : base(message, innerException)
    {
        Login = login;
    }
}