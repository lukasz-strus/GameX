using Flurl.Http;
using gamexModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexServices;

public interface IAuthenticationService
{
    Task<int> ChangePassword(string token, UserPasswordDto userPasswordDto);

    Task<int> Register(RegisterUserDto registerUserDto);

    Task<string> Login(LoginDto loginDto);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly string _baseUrl = "https://gamex-api-app.azurewebsites.net/api/user";

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