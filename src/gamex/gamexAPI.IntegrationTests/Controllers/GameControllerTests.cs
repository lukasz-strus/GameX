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
    // TODO Seed danych, bo GetSerialCode zwraca błędne wartości przez UserTests
    public GameControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    #region Get

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_ForExistId_ReturnOk(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

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
    [MemberData(nameof(GameControllerTestsData.GetSampleValidQueryParams), MemberType = typeof(GameControllerTestsData))]
    public async Task GetAllGames_WithSampleValidQueryParams_ReturnsOk(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(GameControllerTestsData.GetSampleInvalidQueryParams), MemberType = typeof(GameControllerTestsData))]
    public async Task GetAllGames_WithSampleInvalidQueryParams_ReturnsBadRequest(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion GetAll

    #region GetSerialKey

    [Theory]
    [InlineData(1, 2)]
    public async Task GetSerialKey_ForCorrectId_ReturnOk(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage.Should().NotBe(null);
    }

    [Theory]
    [InlineData(2, 1)]
    public async Task GetSerialKey_ForIncorrectParams_ReturnNotAcceptable(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Theory]
    [InlineData(100, 1)]
    [InlineData(1, 100)]
    public async Task GetSerialKey_ForNonExistentId_ReturnNotFound(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion GetSerialKey

    #region Create

    [Theory]
    [MemberData(nameof(GameControllerTestsData.GetSampleInvalidData), MemberType = typeof(GameControllerTestsData))]
    public async Task Create_WithIncorrectQueryParams_ReturnsBadRequest(CreateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(GameControllerTestsData.GetSampleValidData), MemberType = typeof(GameControllerTestsData))]
    public async Task Create_WithTheSameName_ReturnsBadRequest(CreateGameDto model)
    {
        var httpContent = model.ToJsonHttpContent();

        await Client.PostAsync("api/game", httpContent);

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(GameControllerTestsData.GetSampleValidData), MemberType = typeof(GameControllerTestsData))]
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
    [MemberData(nameof(GameControllerTestsData.GetSampleValidQuery), MemberType = typeof(GameControllerTestsData))]
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
    [MemberData(nameof(GameControllerTestsData.GetSampleValidQuery), MemberType = typeof(GameControllerTestsData))]
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

        SeedGame(game);

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

    private void SeedGame(Game game)
    {
        var scopeFactory = Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<GamexDbContext>();

        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();
    }

    #endregion Delete
}