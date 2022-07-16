using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexModels;
using gamexServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexDesktopApp.State.Authenticators;

public enum RoleType
{
    User,
    Seller,
    Admin
}

public interface IAuthenticator
{
    public Account CurrentAccount { get; }
    bool IsLoggedIn { get; }

    event Action StateChanged;

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="login">The user's name.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="confirmPassword">The user's confirmed password.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="confirmEmail">The user's confirmed email.</param>
    /// <param name="total">The user's total money.</param>
    /// <param name="role">The user's role.</param>
    /// <returns>The result of the registration.</returns>
    Task<int> Register(
        string login,
        string password,
        string confirmPassword,
        string email,
        string confirmEmail);

    /// <summary>
    /// Login to the application.
    /// </summary>
    /// <param name="login">The user's name.</param>
    /// <param name="password">The user's password.</param>
    /// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
    /// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
    Task Login(string login, string password);

    void Logout();
}

public class Authenticator : IAuthenticator
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccountStore _accountStore;
    private readonly IUserService _userService;

    public Authenticator(IAuthenticationService authenticationService, IAccountStore accountStore, IUserService userService)
    {
        _authenticationService = authenticationService;
        _accountStore = accountStore;
        _userService = userService;
        CurrentAccount = new Account();
    }

    public Account CurrentAccount
    {
        get
        {
            return _accountStore.CurrentAccount;
        }
        private set
        {
            _accountStore.CurrentAccount = value;
            StateChanged?.Invoke();
        }
    }

    public bool IsLoggedIn => _accountStore != null;

    public event Action StateChanged;

    public async Task Login(string login, string password)
    {
        var loginDto = new LoginDto
        {
            Login = login,
            Password = password
        };

        _accountStore.CurrentAccount.Token = await _authenticationService.Login(loginDto);

        var token = _accountStore.CurrentAccount.Token;
        var userId = GetUserIdFromJwt(token);
        var userDto = await _userService.Get(token, userId);

        AssignValues(userDto);
    }

    public void Logout()
    {
        _accountStore.CurrentAccount.Token = null;
    }

    public async Task<int> Register(
        string login,
        string password,
        string confirmPassword,
        string email,
        string confirmEmail)
    {
        var registerUserDto = new RegisterUserDto
        {
            Login = login,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email,
            ConfirmEmail = confirmEmail
        };

        return await _authenticationService.Register(registerUserDto);
    }

    private int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }

    private void AssignValues(UserDto userDto)
    {
        _accountStore.CurrentAccount.Login = userDto.Login;
        _accountStore.CurrentAccount.Email = userDto.Email;
        _accountStore.CurrentAccount.Total = userDto.Total;
        _accountStore.CurrentAccount.Role = (Role)userDto.RoleId;
    }
}