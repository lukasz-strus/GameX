using gamexAPI.IntegrationTests.Helpers;

namespace gamexAPI.IntegrationTests.Controllers;

[Collection(Constants.TEST_COLLECTION)]
public class UserControllerTests : BaseTest
{
    public UserControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    #region Login and Register

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

    #endregion Login and Register
}

//TODO GetAll, Get, Delete, ChangePassword, Update