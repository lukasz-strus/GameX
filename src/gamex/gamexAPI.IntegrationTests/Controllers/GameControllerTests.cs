using FluentAssertions;
using gamexAPI.IntegrationTests.Data;
using gamexAPI.IntegrationTests.Helpers;
using gamexEntities;
using gamexModels;
using System.Net;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Controllers;

[Collection(Constants.TEST_COLLECTION)]
public class GameControllerTests : BaseTest
{
    public GameControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    #region Get

    [Fact]
    public async Task Get_ForExistId_ReturnOk()
    {
        var game = new Game()
        {
            Id = 1000,
            Name = "Test Delete",
            Description = "Test Delete",
            Price = 100
        };

        Seed(game);

        var response = await Client.GetAsync("api/game/" + game.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Get_ForNonExistentId_ReturnNotFound(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Get

    #region GetAll

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleValidQueryParams), MemberType = typeof(ControllerTestsData))]
    public async Task GetAllGames_WithSampleValidQueryParams_ReturnsOk(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleInvalidQueryParams), MemberType = typeof(ControllerTestsData))]
    public async Task GetAllGames_WithSampleInvalidQueryParams_ReturnsBadRequest(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion GetAll

    #region GetSerialKey

    [Fact]
    public async Task GetSerialKey_ForCorrectId_ReturnOk()
    {
        var game = new Game()
        {
            Id = 1100,
            Name = "Test Delete",
            Description = "Test Delete",
            Price = 100
        };

        var user = new User()
        {
            Id = 1100,
            Login = "Test",
            Email = "test@test.com",
            PasswordHash = "test",
            Total = 1000
        };

        Seed(game, user);

        var response = await Client.GetAsync($"api/game/{user.Id}/{game.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage.Should().NotBe(null);
    }

    [Fact]
    public async Task GetSerialKey_ForIncorrectParams_ReturnNotAcceptable()
    {
        var game = new Game()
        {
            Id = 1200,
            Name = "Test Delete",
            Description = "Test Delete",
            Price = 1000
        };

        var user = new User()
        {
            Id = 1200,
            Login = "Test",
            Email = "test@test.com",
            PasswordHash = "test",
            Total = 100
        };

        Seed(game, user);

        var response = await Client.GetAsync($"api/game/{user.Id}/{game.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Theory]
    [InlineData(12345, 1)]
    [InlineData(1, 12345)]
    public async Task GetSerialKey_ForNonExistentId_ReturnNotFound(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion GetSerialKey

    #region Create

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleInvalidGameData), MemberType = typeof(ControllerTestsData))]
    public async Task Create_WithIncorrectQueryParams_ReturnsBadRequest(CreateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleValidGameData), MemberType = typeof(ControllerTestsData))]
    public async Task Create_WithTheSameName_ReturnsBadRequest(CreateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        await Client.PostAsync("api/game", httpContent);

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleValidGameData), MemberType = typeof(ControllerTestsData))]
    public async Task Create_WithSampleValidData_ReturnsCreated(CreateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    #endregion Create

    #region Update

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleValidUpdateGameData), MemberType = typeof(ControllerTestsData))]
    public async Task Update_WithSampleValidQuery_ReturnsOk(UpdateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        var response = await Client.PutAsync($"api/game/2", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ForTheSameName_ReturnsBadRequest()
    {
        var model = new UpdateGameDto()
        {
            Name = "Test",
            Description = "Test",
            Price = 120m
        };

        var httpContent = model.ToJsonHttpContent();

        await Client.PutAsync($"api/game/2", httpContent);
        var response = await Client.PutAsync($"api/game/2", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(ControllerTestsData.GetSampleValidUpdateGameData), MemberType = typeof(ControllerTestsData))]
    public async Task Update_ForNonExistentId_ReturnsNotFound(UpdateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        var response = await Client.PutAsync($"api/game/100", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task Delete_ForExistGame_ReturnNoContent()
    {
        var game = new Game()
        {
            Id = 25,
            Name = "Test Delete",
            Description = "Test Delete",
            Price = 100
        };

        Seed(game);

        var response = await Client.DeleteAsync("api/game/" + game.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Delete_ForNonExistentGame_ReturnNotFound(int id)
    {
        var response = await Client.DeleteAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Delete
}