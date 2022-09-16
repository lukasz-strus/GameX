using FluentAssertions;
using gamexAPI.IntegrationTests.Helpers;
using gamexModels;
using Moq;
using System.Net;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Controllers;

public class UserControllerTests : BaseTest
{
    public UserControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    //TODO dopisać resztę testów

    [Fact]
    public async Task Login_ForRegisteredUser_ReturnsOk()
    {
        Factory.UserService
            .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
            .Returns("token");

        var loginDto = new LoginDto()
        {
            Login = "login",
            Password = "password123"
        };

        var httpContent = loginDto.ToJsonHttpContent();

        var response = await Client.PostAsync("api/user/login", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ForValidModel_ReturnsOk()
    {
        var registerUser = new RegisterUserDto()
        {
            Login = "login",
            Password = "password123",
            ConfirmPassword = "password123",
            Email = "test@test.com",
            ConfirmEmail = "test@test.com"
        };

        var httpContent = registerUser.ToJsonHttpContent();

        var response = await Client.PostAsync("api/user", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ForInvalidModel_ReturnsBadRequest()
    {
        var registerUser = new RegisterUserDto()
        {
            Password = "password123",
            ConfirmPassword = "1",
            Email = "test@test.com",
            ConfirmEmail = "test1@test.com"
        };

        var httpContent = registerUser.ToJsonHttpContent();

        var response = await Client.PostAsync("api/user", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}