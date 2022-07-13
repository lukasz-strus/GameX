using Flurl.Http;
using gamexModelsDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexServices;

public interface IAuthenticationService
{
    /// <summary>
    /// Change password by user Id
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="userPasswordDto">User password data transfer object</param>
    /// <returns>Status Code</returns>
    Task<int> ChangePassword(string token, UserPasswordDto userPasswordDto);

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registerUserDto">Register user data transfer object</param>
    /// <returns>Status code</returns>
    Task<int> Register(RegisterUserDto registerUserDto);

    /// <summary>
    /// Log In
    /// </summary>
    /// <param name="loginDto">Login data transfer object</param>
    /// <returns>Token JWT</returns>
    Task<string> Login(LoginDto loginDto);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly string _baseUrl =
"https://gamex-api-app.azurewebsites.net/api/user";

    public async Task<int> Register(RegisterUserDto registerUserDto)
    {
        var response = await _baseUrl
            .PostJsonAsync(registerUserDto);

        return response.StatusCode;
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var response = await (_baseUrl + "/login")
            .PostJsonAsync(loginDto)
            .ReceiveString();

        return response;
    }

    public async Task<int> ChangePassword(string token, UserPasswordDto userPasswordDto)
    {
        var userId = GetUserIdFromJwt(token);

        var response = await (_baseUrl + $"/{userId}")
            .WithOAuthBearerToken(token)
            .PutJsonAsync(userPasswordDto);

        return response.StatusCode;
    }

    private int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }
}